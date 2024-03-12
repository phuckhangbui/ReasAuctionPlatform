import { Button, Carousel, Typography } from "@material-tailwind/react";
import { useContext, useEffect, useState } from "react";
import { getRealEstateById } from "../../api/realEstate";
import { NumberFormat } from "../../utils/numbetFormat";
import { Empty, InputNumber, Modal, Statistic } from "antd";
import { Button as ButtonAnt } from "antd";
import { UserContext } from "../../context/userContext";
import dayjs from "dayjs";
import { set, ref, get, child, update, onValue } from "firebase/database";
import { db } from "../../Config/firebase-config";
import { getAuctionHome, getAuctionUserList } from "../../api/memberAuction";

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
  const [currentBid, setCurrentBid] = useState(0);
  const [currentInputBid, setCurrentInputBid] = useState(currentBid);
  const [currentAutoBidValue, setCurrentAutoBidValue] = useState(0);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isAuctionOpen, setAuctionOpen] = useState(true);
  const [checked, setChecked] = useState(false);
  const [isInputValid, setIsInputValid] = useState(false);
  const { isAuth } = useContext(UserContext);
  const [dateEnd, setDateEnd] = useState<any>();
  const [auction, setAuction] = useState<memberAuction | undefined>();
  const [totalUsers, setTotalUsers] = useState(0);
  const [isUsersAttend, setIsUserAttend] = useState(false);
  const [isOneParticipant, setIsOneParticipant] = useState(true);
  const [countdownValue, setCountdownValue] = useState(60000);
  const [userRegisterList, setUserRegisterList] = useState<
    number[] | undefined
  >();
  const [isUserRegistered, setIsUserRegistered] = useState(false);

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
      const fetchRealEstates = async () => {
        const response = await getAuctionUserList(realEstateId);
        setUserRegisterList(response);
      };
      fetchRealEstates();
      const userId = localStorage.getItem("userId");
      if (userId) {
        if (userRegisterList?.includes(parseInt(userId))) {
          setIsUserRegistered(true);
        }
      }
    } catch (error) {
      console.log(error);
    }
  }, []);

  useEffect(() => {
    try {
      const fetchCurrentBid = async () => {
        const currentBidRef = ref(db, `auctions/${realEstateId}/currentBid`);
        const snapshot = await get(currentBidRef);
        if (snapshot.exists()) {
          const currentBidValue = snapshot.val();
          setCurrentBid(currentBidValue);
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
            if (realEstateId) {
              const response = await getAuctionHome(realEstateId);
              setAuction(response);
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
    if (!auction) {
      return;
    }
    const auctionRef = ref(db, `auctions/${auction.reasId}`);
    const usersRef = ref(db, `auctions/${auction.reasId}/users`);
    const currentBidRef = ref(db, `auctions/${auction.reasId}/currentBid`);
    const userId = localStorage.getItem("userId");

    var userAlreadyRegistered = false;

    if (userId && userRegisterList) {
      userAlreadyRegistered = userRegisterList.includes(parseInt(userId));
    }

    get(child(auctionRef, "auction"))
      .then((snapshot) => {
        const auctionData = snapshot.exists() ? snapshot.val() : null;
        const status = userAlreadyRegistered ? (snapshot.exists() ? 2 : 1) : 0;

        if (!auctionData) {
          update(auctionRef, {
            auction,
            currentBid: auction.floorBid,
            status,
            lastBid: auction.dateStart,
          });
        }

        if (!userId) return;

        if (!userAlreadyRegistered) return;
        
        get(usersRef)
          .then((usersSnapshot) => {
            if (!usersSnapshot.hasChild(userId)) {
              update(usersRef, {
                [userId]: {
                  userId,
                  currentUserBid: 0,
                },
              });
              setIsUserRegistered(true);
            } else {
              setIsUserRegistered(true);
            }
          })
          .catch((error) => {
            console.error(
              "Error checking user existence in the auction:",
              error
            );
          });

        get(currentBidRef)
          .then((currentBidValue) => {
            if (currentBidValue.exists()) {
              const value = currentBidValue.val();
              setCurrentBid(value);
              setCurrentInputBid(value);
            }
          })
          .catch((error) => {
            console.error("Error checking current bid in the auction:", error);
          });
      })
      .catch((error) => {
        console.error("Error checking auction existence:", error);
      });
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

  const handleBid = async () => {
    try {
      const userId = localStorage.getItem("userId");
      // Update the currentBid attribute in Firebase
      const auctionRef = ref(db, `auctions/${auction?.reasId}`);
      await update(auctionRef, {
        currentBid: currentInputBid,
        lastBid: Date.now(),
      });

      const userBidRef = ref(db, `auctions/${auction?.reasId}/users/${userId}`);
      await update(userBidRef, {
        currentUserBid: currentInputBid,
      });

      setCurrentBid(currentInputBid);
    } catch (error) {
      console.error("Error updating bid in Firebase:", error);
    }
  };

  useEffect(() => {
    const lastBidRef = ref(db, `auctions/${realEstateId}/lastBid`);
    const unsubscribe = onValue(lastBidRef, (snapshot) => {
      const lastBid = snapshot.val();
      const newCountdownValue = lastBid + 60000;
      setCountdownValue(newCountdownValue);
    });

    // Clean up the listener when component unmounts
    return () => {
      unsubscribe();
    };
  }, [realEstateId]);

  useEffect(() => {
    const currentBidRef = ref(db, `auctions/${realEstateId}/currentBid`);
    const unsubscribe = onValue(currentBidRef, (snapshot) => {
      const newBid = snapshot.val();
      setCurrentBid(newBid);
      setCurrentInputBid(newBid);
      const newCountdownValue = currentBid === 0 ? 60000 : Date.now() + 10000;
      setCountdownValue(newCountdownValue);
    });

    // Clean up the listener when component unmounts
    return () => {
      unsubscribe();
    };
  }, [realEstateId]);

  useEffect(() => {
    const usersRef = ref(db, `auctions/${realEstateId}/users`);
    const unsubscribe = onValue(usersRef, (snapshot) => {
      let usersList = 0;
      snapshot.forEach(() => {
        usersList++;
      });
      setTotalUsers(usersList);
      if (usersList >= 2) {
        setIsOneParticipant(false);
        setIsUserAttend(false);
        setAuctionOpen(false);
      }
      if (usersList == 0) {
        setIsOneParticipant(false);
        setIsUserAttend(false);
        setAuctionOpen(false);
      }
      if (usersList == 1) {
        setIsOneParticipant(true);
        setIsUserAttend(false);
        setAuctionOpen(false);
      }
    });
    return () => {
      unsubscribe();
    };
  }, [realEstateId]);

  useEffect(() => {
    const statusRef = ref(db, `auctions/${realEstateId}/status`);
    const unsubscribe = onValue(statusRef, (snapshot) => {
      const statusValue = snapshot.val();
      if (statusValue == 4) {
        setAuctionOpen(true);
      } else {
        setAuctionOpen(false);
      }
    });
    return () => {
      unsubscribe();
    };
  }, [realEstateId]);

  const handleOnFinish = () => {
    if (auction) {
      if (currentBid > auction?.floorBid) {
        const auctionRef = ref(db, `auctions/${realEstateId}`);
        update(auctionRef, {
          status: 4,
        });
      }
    }
  };

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
            {auction ? (
              <>
                {isAuctionOpen ? (
                  <>
                    <Typography variant="h3" className="text-center">
                      true
                    </Typography>
                  </>
                ) : (
                  <>
                    <Typography variant="h3" className="text-center">
                      false
                    </Typography>
                  </>
                )}

                <div>
                  <Typography variant="h3" className="text-center">
                    Current bid: {NumberFormat(currentBid)}
                  </Typography>
                  <Countdown
                    value={countdownValue}
                    format=" m [minutes] s [secs]"
                    onFinish={() => handleOnFinish()}
                  />
                </div>

                <div className="grid grid-cols-5 gap-4">
                  <div className="col-span-3">
                    <Typography variant="h5">
                      Auction start:{" "}
                      {dayjs(auction?.dateStart).format("DD/MM/YYYY HH:mm:ss")}
                    </Typography>
                    <div className="font-semibold flex items-center">
                      Total user: {totalUsers}
                    </div>
                    <Countdown
                      value={dayjs(auction.dateEnd).toDate().toString()}
                      format="H [hours] m [minutes] s [secs]"
                    />
                  </div>
                  {!isAuthenticated ? (
                    <>
                      <Typography>Please login first</Typography>
                    </>
                  ) : (
                    <>
                      {isUserRegistered ? (
                        <>
                          {!isOneParticipant ? (
                            <>
                              {isUsersAttend ? (
                                <>
                                  {isAuctionOpen ? (
                                    <>
                                      {" "}
                                      <div className="col-span-2 flex flex-col space-y-4">
                                        <div className="flex space-x-4">
                                          <div className="flex">
                                            <Button
                                              size="sm"
                                              onClick={() => {
                                                setCurrentInputBid(
                                                  currentInputBid - 1000000
                                                );
                                              }}
                                              disabled={
                                                currentInputBid === currentBid
                                              }
                                            >
                                              -
                                            </Button>
                                            <InputNumber
                                              style={{
                                                width: "auto",
                                              }}
                                              value={NumberFormat(
                                                currentInputBid
                                              )}
                                            />
                                            <Button
                                              size="sm"
                                              onClick={() => {
                                                setCurrentInputBid(
                                                  currentInputBid + 1000000
                                                );
                                              }}
                                            >
                                              +
                                            </Button>
                                          </div>
                                          <Button
                                            onClick={handleBid}
                                            disabled={
                                              currentInputBid === currentBid
                                            }
                                          >
                                            Bid
                                          </Button>
                                        </div>
                                        <div className="flex flex-row justify-center w-full items-center space-x-4">
                                          <Button onClick={showModal}>
                                            Set auto bid
                                          </Button>
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
                                                <Button
                                                  size="sm"
                                                  onClick={handleDecrease}
                                                >
                                                  -
                                                </Button>
                                                <InputNumber
                                                  className="w-full"
                                                  value={NumberFormat(
                                                    currentAutoBidValue
                                                  )}
                                                />
                                                <Button
                                                  size="sm"
                                                  onClick={handleIncrease}
                                                >
                                                  +
                                                </Button>
                                              </div>
                                              <div>
                                                <ButtonAnt
                                                  type="default"
                                                  size="small"
                                                  onClick={toggleChecked}
                                                >
                                                  {!checked
                                                    ? "Enable"
                                                    : "Unable"}
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
                                  ) : (
                                    <>
                                      <Typography>Auction complete</Typography>
                                    </>
                                  )}
                                </>
                              ) : (
                                <>
                                  <Typography>
                                    Auction will start in 5 minutes
                                  </Typography>
                                </>
                              )}
                            </>
                          ) : (
                            <>
                              <Typography>
                                Waiting for users in 10 minutes.
                              </Typography>
                              <Typography>
                                After 10 minutes, no more users. You win
                              </Typography>
                            </>
                          )}
                        </>
                      ) : (
                        <>
                          <Typography>
                            You didn't register this auction
                          </Typography>
                        </>
                      )}
                    </>
                  )}
                </div>
              </>
            ) : (
              <>
                <Empty />
              </>
            )}
          </div>
        </div>
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
