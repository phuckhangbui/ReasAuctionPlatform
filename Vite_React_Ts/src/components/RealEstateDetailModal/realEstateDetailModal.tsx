import { Button, Carousel, Input, Typography } from "@material-tailwind/react";
import { useContext, useEffect, useRef, useState } from "react";
import { getRealEstateById } from "../../api/realEstate";
import { NumberFormat } from "../../utils/numbetFormat";
import { InputNumber, Modal, Statistic } from "antd";
import { Button as ButtonAnt } from "antd";
import { EyeOutlined } from "@ant-design/icons";
import { UserContext } from "../../context/userContext";
import dayjs from "dayjs";
import { set, ref, get, child, update, onValue } from "firebase/database";
import { db } from "../../Config/firebase-config";
import { getAuctionHome, getAuctionStatus } from "../../api/memberAuction";
import LoginModal from "../LoginModal/loginModal";

interface RealEstateDetailModalProps {
  realEstateId: number;
  closeModal: () => void;
  address: string;
  index: string;
}

const { Countdown } = Statistic;

const RealEstateDetailModal = ({
  closeModal,
  realEstateId,
  index,
}: RealEstateDetailModalProps) => {
  const [tabStatus, setTabStatus] = useState(index);
  const [currentBid, setCurrentBid] = useState(100000);
  const [currentInputBid, setCurrentInputBid] = useState(currentBid);
  const [currentAutoBidValue, setCurrentAutoBidValue] = useState(0);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [checked, setChecked] = useState(false);
  const [isInputValid, setIsInputValid] = useState(false);
  const { isAuth, token, userId } = useContext(UserContext);
  const [dateEnd, setDateEnd] = useState<any>();
  const [auction, setAuction] = useState<memberAuction | undefined>();
  const [auctionStatus, setAuctionStatus] = useState<number>();
  const [showLogin, setShowLogin] = useState(false);
  // 0: RealEstate not in selling status
  // 1: Not register in auction
  // 2: Register but pending payment
  // 3: Register success
  // 4: User is the owner of real estate

  // Use the isAuth function to determine if the user is authenticated
  const isAuthenticated = isAuth();

  const [realEstateDetail, setRealEstateDetail] = useState<
    realEstateDetail | undefined
  >();

  useEffect(() => {
    try {
      const fetchRealEstateDetail = async () => {
        const response = await getRealEstateById(realEstateId);
        setRealEstateDetail(response);
      };

      fetchRealEstateDetail();
    } catch (error) {
      console.log(error);
    }
  }, []);

  useEffect(() => {
    try {
      const fetchAuctionStatus = async () => {
        if (userId && token) {
          if (realEstateDetail?.reasId) {
            const response = await getAuctionStatus(
              userId,
              realEstateDetail.reasId,
              token
            );
            if (response) {
              setAuctionStatus(response?.status);
            }
          }
        }
      };
      fetchAuctionStatus();
    } catch (error) {}
  }, [realEstateDetail]);

  useEffect(() => {
    try {
      const fetchCurrentBid = async () => {
        const currentBidRef = ref(db, `auctions/${realEstateId}/currentBid`);
        const snapshot = await get(currentBidRef);
        if (snapshot.exists()) {
          const currentBidValue = snapshot.val();
          setCurrentBid(currentBidValue);
        } else {
          console.log("Current bid not found in Firebase");
        }
      };
      fetchCurrentBid();
    } catch (error) {}
  });

  useEffect(() => {
    try {
      const fetchRealEstates = async () => {
        try {
          const auctionRef = ref(db, `auctions/${realEstateId}`);
          const snapshot = await get(auctionRef);
          if (snapshot.exists()) {
            setAuction(snapshot.val().auction);
          } else {
            console.log("Auction not found in Firebase");
            if (realEstateId) {
              const response = await getAuctionHome(realEstateId);
              setAuction(response);
            } else {
              console.log("not found id");
            }
          }
        } catch (error) {
          console.error("Error fetching auction data:", error);
        }
      };

      fetchRealEstates();
    } catch (error) {
      console.log(error);
    }
  }, []);

  const dateEndFormat = (dateEnd: any) => {
    setDateEnd(dayjs(dateEnd).format("DD/MM/YYYY HH:mm:ss"));
  };
  useEffect(() => {
    try {
      if (realEstateDetail?.dateEnd) {
        dateEndFormat(realEstateDetail?.dateEnd);
      }
    } catch (error) {
      console.log(error);
    }
  }, [realEstateDetail?.dateEnd]);

  // Change the tab index
  const toggleTab = (index: string) => {
    setTabStatus(index);
  };

  const toggleAuction = (index: string, auction: memberAuction | undefined) => {
    setTabStatus(index);
    if (auction) {
      const auctionRef = ref(db, `auctions/${auction.reasId}`);
      const userId = localStorage.getItem("userId");
      get(child(auctionRef, "auction"))
        .then((snapshot) => {
          if (snapshot.exists()) {
            // Auction already exists
            console.log("Auction already exists:", auction.reasId);
            // Check if user exists in the auction
            const usersRef = ref(db, `auctions/${auction.reasId}/users`);
            get(usersRef)
              .then((usersSnapshot) => {
                if (!userId) {
                  console.error(
                    "User ID not found in local storage or is null"
                  );
                  return;
                }
                if (!usersSnapshot.hasChild(userId)) {
                  // User doesn't exist, add the new user
                  console.log("Adding new user to the auction:", userId);
                  update(ref(db, `auctions/${auction.reasId}/users`), {
                    [userId]: {
                      userId: userId,
                      currentUserBid: 0,
                    },
                  });
                  // Notify all users in the userList
                  usersSnapshot.forEach((childSnapshot) => {
                    const userKey = childSnapshot.key;
                    console.log(
                      `New user ${userId} has been added to the auction by ${userKey}`
                    );
                  });
                } else {
                  console.log("UserId already exists in the auction:", userId);
                }
              })
              .catch((error) => {
                console.error(
                  "Error checking user existence in the auction:",
                  error
                );
              });
          } else {
            // Auction doesn't exist, add it to Firebase
            console.log(
              "Auction doesn't exist. Adding it to Firebase:",
              auction.reasId
            );
            set(ref(db, `auctions/${auction.reasId}`), {
              auction,
              users: {
                [localStorage.getItem("userId") || ""]: {
                  userId: userId,
                  currentUserBid: 0,
                },
              },
              currentBid: auction.floorBid,
            });
          }
        })
        .catch((error) => {
          console.error("Error checking auction existence:", error);
        });
    } else {
      console.log("not found");
    }
  };
  // Tab button status
  const getActiveTab = (index: string) => {
    return `${
      index === tabStatus
        ? "text-mainBlue border-mainBlue font-bold"
        : "border-transparent hover:border-gray-900"
    } text-xl  border-b-2 rounded-t-lg`;
  };

  // Changing the tab description
  const getActiveTabDetail = (index: string) => {
    return `${index === tabStatus ? "" : "hidden"} mt-2 space-y-4 `;
  };

  const showModal = () => {
    setIsModalOpen(true);
  };

  const handleOk = () => {
    setIsModalOpen(false);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const handleDecrease = () => {
    setCurrentAutoBidValue(currentAutoBidValue - 1000); // Decrease currentBid by 1
  };

  const handleIncrease = () => {
    setCurrentAutoBidValue(currentAutoBidValue + 1000); // Increase currentBid by 1
  };

  const toggleLogin = () => {
    setShowLogin((prevLogIn) => !prevLogIn);
  };

  const handleOverlayClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) {
      // If the click occurs on the overlay (not on the modal content), close the modal
      closeLogin();
    }
  };

  const closeLogin = () => {
    setShowLogin(!showLogin);
  };

  const handleBid = async () => {
    try {
      // Update the currentBid attribute in Firebase
      const auctionRef = ref(db, `auctions/${auction?.reasId}`);
      await update(auctionRef, {
        currentBid: currentInputBid,
      });

      // Optionally, you can also update the currentInputBid state locally if needed
      setCurrentBid(currentInputBid);

      // Optionally, you can perform any other actions after the bid is successfully updated in Firebase
    } catch (error) {
      console.error("Error updating bid in Firebase:", error);
    }
  };

  useEffect(() => {
    // Set up listener for changes to currentBid attribute in Firebase
    const auctionRef = ref(db, `auctions/${realEstateId}/currentBid`);
    const unsubscribe = onValue(auctionRef, (snapshot) => {
      const newBid = snapshot.val();
      setCurrentBid(newBid);
    });

    // Clean up the listener when component unmounts
    return () => {
      unsubscribe(); // Unsubscribe from updates to prevent memory leaks
    };
  }, [realEstateId]);

  const toggleChecked = () => {
    if (checked == true) {
      setChecked(!checked);
    } else {
      if (currentAutoBidValue > 0) {
        setIsInputValid(true); // Set validation state to true if input value is larger than 0
        setChecked(!checked);
      } else {
        setIsInputValid(false); // Set validation state to false otherwise
      }
    }
  };
  return (
    <div className="relative w-full max-w-7xl max-h-full ">
      <div className="relative bg-white rounded-lg shadow md:px-10 md:pb-5 sm:px-0 sm:pb-0 ">
        <div className=" items-center justify-start md:py-5 md:px-0 sm:p-5 sm:fixed md:static z-10 top-0">
          <button
            type="button"
            className=" bg-transparent md:bg-transparent sm:bg-white sm:bg-opacity-60 rounded-3xl text-sm w-10 h-10 ms-auto inline-flex justify-center items-center "
            data-modal-hide="default-modal"
            onClick={closeModal}
          >
            <svg
              className="w-6 h-6  sm:text-black sm:hover:text-mainBlue "
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 14 10"
            >
              <path
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M13 5H1m0 0 4 4M1 5l4-4"
              />
            </svg>
            <span className="sr-only">Close modal</span>
          </button>
        </div>

        <div className="md:mx-24 ">
          <div className="top-5">
            <Carousel className=" rounded-lg">
              {realEstateDetail?.photos?.map((photo) => (
                <img
                  src={photo.reasPhotoUrl}
                  alt="Real Estate Photos"
                  className="md:h-120 sm:h-96 w-full object-fill rounded-lg"
                  key={photo.reasPhotoId}
                />
              ))}
            </Carousel>
          </div>
        </div>
        <hr className="mt-8 mb-6 border-gray-200 sm:mx-auto " />
        <div className=" md:mb-0 sm:px-4 lg:px-30">
          <div className="">
            <div className="text-4xl font-bold text-justify">
              {realEstateDetail?.reasName}
            </div>
            <div>
              <ul className="mt-2 flex flex-row gap-4">
                <li>
                  <button
                    className={getActiveTab("detail")}
                    onClick={() => toggleTab("detail")}
                  >
                    Detail
                  </button>
                </li>
                <li>
                  <button
                    className={getActiveTab("auction")}
                    onClick={() => toggleAuction("auction", auction)}
                  >
                    Auction
                  </button>
                </li>
              </ul>
            </div>
          </div>
          <div className={getActiveTabDetail("detail")}>
            <div className="grid md:grid-cols-4 sm:grid-cols-2 gap-2">
              <div className=" md:col-span-2 flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M8 17.3a5 5 0 0 0 2.6 1.7c2.2.6 4.5-.5 5-2.3.4-2-1.3-4-3.6-4.5-2.3-.6-4-2.7-3.5-4.5.5-1.9 2.7-3 5-2.3 1 .2 1.8.8 2.5 1.6m-3.9 12v2m0-18v2.2"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.reasPrice},000 VND
                  </div>
                  <div className="text-xs text-gray-700">Starting Price</div>
                </div>
              </div>
              <div className=" flex items-center ">
                <div className="">
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M9 7H7m2 3H7m2 3H7m4 2v2m3-2v2m3-2v2M4 5v14c0 .6.4 1 1 1h14c.6 0 1-.4 1-1v-3c0-.6-.4-1-1-1h-9a1 1 0 0 1-1-1V5c0-.6-.4-1-1-1H5a1 1 0 0 0-1 1Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.reasArea}
                  </div>
                  <div className="text-xs text-gray-700">Sqft</div>
                </div>
              </div>
              <div className="flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-width="2"
                      d="M7 17v1c0 .6.4 1 1 1h8c.6 0 1-.4 1-1v-1a3 3 0 0 0-3-3h-4a3 3 0 0 0-3 3Zm8-9a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.accountOwnerName}
                  </div>
                  <div className="text-xs text-gray-700">Uploaded by</div>
                </div>
              </div>
              <div className="md:col-span-2  flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M6 4h12M6 4v16M6 4H5m13 0v16m0-16h1m-1 16H6m12 0h1M6 20H5M9 7h1v1H9V7Zm5 0h1v1h-1V7Zm-5 4h1v1H9v-1Zm5 0h1v1h-1v-1Zm-3 4h2a1 1 0 0 1 1 1v4h-4v-4a1 1 0 0 1 1-1Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.reasAddress}
                  </div>
                  <div className="text-xs text-gray-700">Property address</div>
                </div>
              </div>
              <div className=" flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="m4 12 8-8 8 8M6 10.5V19c0 .6.4 1 1 1h3v-3c0-.6.4-1 1-1h2c.6 0 1 .4 1 1v3h3c.6 0 1-.4 1-1v-8.5"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.type_REAS_Name}
                  </div>
                  <div className="text-xs text-gray-700">Property type</div>
                </div>
              </div>
              <div className=" flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M4 10h16m-8-3V4M7 7V4m10 3V4M5 20h14c.6 0 1-.4 1-1V7c0-.6-.4-1-1-1H5a1 1 0 0 0-1 1v12c0 .6.4 1 1 1Zm3-7h0v0h0v0Zm4 0h0v0h0v0Zm4 0h0v0h0v0Zm-8 4h0v0h0v0Zm4 0h0v0h0v0Zm4 0h0v0h0v0Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">{dateEnd}</div>
                  <div className="text-xs text-gray-700">Due date</div>
                </div>
              </div>
            </div>
            <div className="">
              <div className="text-xl font-bold ">Description</div>
              <div
                dangerouslySetInnerHTML={{
                  __html: realEstateDetail?.reasDescription || "",
                }}
                className="mt-1"
              ></div>
            </div>
          </div>
          <div className={getActiveTabDetail("auction")}>
            {userId ? (
              auctionStatus === 0 ? ( // Not available for selling
                <div className="flex justify-center p-10">
                  <div className="text-4xl text-red-700">
                    Real Estate Is Currently Not Available For Selling
                  </div>
                </div>
              ) : auctionStatus === 1 ? (
                <div className="flex justify-center">
                  <button
                  
                  className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                    Register
                  </button>
                </div>
              ) : auctionStatus === 2 ? (
                <div className="flex justify-center py-10">
                  <div className="text-xl text-gray-500">
                    Payment is currently pending, please check your history in
                    the profile section for more information
                  </div>
                </div>
              ) : auctionStatus === 3 ? (
                <div>
                  <Typography variant="h3" className="text-center">
                    Current bid: {NumberFormat(currentBid)}
                  </Typography>
                  <div className="grid grid-cols-5 gap-4">
                    {!isAuthenticated ? (
                      <div className="col-span-3">
                        <Typography variant="h5">
                          Auction ends:{" "}
                          {dayjs(auction?.dateEnd).format(
                            "DD/MM/YYYY HH:mm:ss"
                          )}
                        </Typography>
                        <div className="font-semibold flex items-center">
                          <ButtonAnt
                            type="default"
                            shape="circle"
                            icon={<EyeOutlined />}
                          ></ButtonAnt>
                          Add to watch list
                        </div>
                        <Countdown
                          value={auction?.dateEnd}
                          format="D [days] H [hours] m [minutes] s [secs]"
                        />
                      </div>
                    ) : (
                      <>
                        <div className="col-span-3">
                          <Typography variant="h5">
                            Auction ends:{" "}
                            {dayjs(auction?.dateEnd).format(
                              "DD/MM/YYYY HH:mm:ss"
                            )}
                          </Typography>
                          <div className="font-semibold flex items-center">
                            <ButtonAnt
                              type="default"
                              shape="circle"
                              icon={<EyeOutlined />}
                            ></ButtonAnt>
                            Add to watch list
                          </div>
                          <Countdown
                            value={auction?.dateEnd}
                            format="D [days] H [hours] m [minutes] s [secs]"
                          />
                        </div>
                        <div className="col-span-2 flex flex-col space-y-4">
                          <div className="flex space-x-4">
                            <div className="flex">
                              <Button
                                size="sm"
                                onClick={() => {
                                  setCurrentInputBid(currentInputBid - 1000);
                                }}
                              >
                                -
                              </Button>
                              <InputNumber
                                style={{
                                  width: "auto",
                                }}
                                value={NumberFormat(currentInputBid)}
                              />
                              <Button
                                size="sm"
                                onClick={() => {
                                  setCurrentInputBid(currentInputBid + 1000);
                                }}
                              >
                                +
                              </Button>
                            </div>

                            <Button onClick={handleBid}>Bid</Button>
                          </div>
                          <div className="flex flex-row justify-center w-full items-center space-x-4">
                            <Button onClick={showModal}>Set auto bid</Button>
                            <Typography variant="h6">
                              {NumberFormat(currentAutoBidValue)}
                            </Typography>
                            <Modal
                              title="Auto Bid"
                              open={isModalOpen}
                              onOk={handleOk}
                              okType={"default"}
                              onCancel={handleCancel}
                              width={300}
                            >
                              <div className="space-y-4">
                                <div className="flex">
                                  <Button size="sm" onClick={handleDecrease}>
                                    -
                                  </Button>
                                  <InputNumber
                                    className="w-full"
                                    value={NumberFormat(currentAutoBidValue)}
                                  />
                                  <Button size="sm" onClick={handleIncrease}>
                                    +
                                  </Button>
                                </div>
                                <div>
                                  <ButtonAnt
                                    type="default"
                                    size="small"
                                    onClick={toggleChecked}
                                  >
                                    {!checked ? "Enable" : "Unable"}
                                  </ButtonAnt>
                                </div>
                                {!isInputValid && (
                                  <div className="text-red-500">
                                    Input must be larger than 0!
                                  </div>
                                )}
                              </div>
                            </Modal>
                          </div>
                        </div>
                      </>
                    )}
                  </div>
                </div>
              ) : (
                <div>Users List</div>
              )
            ) : (
              <div className="flex justify-center">
                <button
                  onClick={() => toggleLogin()}
                  className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                >
                  Sign In
                </button>
              </div>
            )}
          </div>
        </div>
        {showLogin && (
          <div
            id="login-modal"
            tabIndex={-1}
            aria-hidden="true"
            className="fixed top-0 left-0 right-0 inset-0 overflow-x-hidden overflow-y-auto z-50 flex items-center justify-center bg-black bg-opacity-50 w-full max-h-full md:inset-0 "
            onMouseDown={handleOverlayClick}
          >
            <LoginModal closeModal={closeModal} />
          </div>
        )}
        <hr className="my-6 border-gray-200 sm:mx-auto lg:my-8 " />
        <footer>
          <div className="w-full max-w-screen-xl">
            <span className="block text-sm text-gray-900 sm:text-center xs:text-center sm:pb-8 md:pb-0">
              © 2023 REAS™ . All Rights Reserved.
            </span>
          </div>
        </footer>
      </div>
    </div>
  );
};

export default RealEstateDetailModal;
