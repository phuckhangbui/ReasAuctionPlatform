import { Button, Carousel, Typography } from "@material-tailwind/react";
import { useContext, useEffect, useState } from "react";
import { getRealEstateById } from "../../api/realEstate";
import { NumberFormat } from "../../Utils/numbetFormat";
import { InputNumber, Statistic } from "antd";
import { UserContext } from "../../context/userContext";
import dayjs, { Dayjs } from "dayjs";
import { ref, get, child, update, onValue } from "firebase/database";
import { db } from "../../Config/firebase-config";
import {
  StartAuction,
  auctionSuccess,
  getAuctionHome,
  getAuctionStatus,
  getAuctionUserList,
} from "../../api/memberAuction";
import { DepositContext } from "../../context/depositContext";
import { registerParticipateAuction } from "../../api/transaction";
import LoginModal from "../LoginModal/loginModal";
import { memberRealEstateDetail } from "../../api/memberRealEstate";

interface RealEstateDetailModalProps {
  realEstateId: number;
  closeModal: () => void;
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
  const [isAuctionOpen, setIsAuctionOpen] = useState(true);
  const [dateEnd, setDateEnd] = useState<any>();
  const [auction, setAuction] = useState<memberAuction | undefined>();
  const [totalUsers, setTotalUsers] = useState(0);
  const [isUsersAttend, setIsUserAttend] = useState(false);
  const [isOneParticipant, setIsOneParticipant] = useState(true);
  const [countDownWaiting, setCountDownWaiting] = useState<Dayjs | undefined>();
  const [userRegisterList, setUserRegisterList] = useState<
    number[] | undefined
  >();
  const [_isUserRegistered, setIsUserRegistered] = useState(false);
  const [realEstateDetail, setRealEstateDetail] = useState<
    realEstateDetail | undefined
  >();
  const [showLogin, setShowLogin] = useState(false);
  const [auctionStatus, setAuctionStatus] = useState<number>();
  // 0: RealEstate not in selling status
  // 1: Not register in auction
  // 2: Register but pending payment
  // 3: Register success
  // 4: User is the owner of real estate
  // 5: Reas is auctioning

  const [isAuctionEnd, setIsAuctionEnd] = useState(false);
  const { getDeposit } = useContext(DepositContext);
  const { token, userId } = useContext(UserContext);

  //get reasDetail
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
      if (userId === realEstateDetail?.accountOwnerId) {
        const fetchOwnerRealEstateDetail = async () => {
          if (token) {
            const response = await memberRealEstateDetail(realEstateId, token);
            setRealEstateDetail(response);
          }
        };
        fetchOwnerRealEstateDetail();
      }
    } catch (error) {
      console.log(error);
    }
  }, [realEstateDetail?.detail === null]);

  //get userRegisterList
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

  //get auction status
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
              console.log(response?.status);
              setAuctionStatus(response?.status);
            }
          }
        }
      };
      fetchAuctionStatus();
    } catch (error) {}
  }, [realEstateDetail]);

  //get real-time currentBid
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

  //get auction
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

  //add auction to firebase
  const toggleAuction = (index: string, auction: memberAuction | undefined) => {
    setTabStatus(index);
    if (!auction) {
      return;
    }
    const auctionRef = ref(db, `auctions/${auction.reasId}`);
    const usersRef = ref(db, `auctions/${auction.reasId}/users`);
    const currentBidRef = ref(db, `auctions/${auction.reasId}/currentBid`);
    const statusRef = ref(db, `auctions/${realEstateId}/status`);
    const userId = localStorage.getItem("userId");

    var userAlreadyRegistered = false;

    if (userId && userRegisterList) {
      userAlreadyRegistered = userRegisterList.includes(parseInt(userId));
    }

    get(child(auctionRef, "auction"))
      .then((snapshot) => {
        const auctionData = snapshot.exists() ? snapshot.val() : null;
        const status = 0;

        if (!auctionData) {
          update(auctionRef, {
            auction,
            currentBid: auction.floorBid,
            status,
            lastBid: auction.dateStart,
            statusChangeTime: Date.now(),
            isBidded: 0,
          });
        }

        if (!userId) return;

        if (!userAlreadyRegistered) return;

        get(statusRef).then((statusSnapshot) => {
          if (statusSnapshot.exists()) {
            if (statusSnapshot.val() == 4) {
              return;
            }
          }
        });

        get(usersRef)
          .then((usersSnapshot) => {
            if (!usersSnapshot.hasChild(userId)) {
              update(usersRef, {
                [userId]: {
                  userId,
                  currentUserBid: 0,
                },
              });
              if (totalUsers == 0) {
                update(auctionRef, {
                  status: 1,
                  statusChangeTime: Date.now(),
                });
                setCountDownWaiting(
                  dayjs(auction.dateStart).add(10, "minutes")
                );
              }
              if (totalUsers == 1) {
                update(auctionRef, {
                  status: 2,
                  statusChangeTime: Date.now(),
                });
              }
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

  //identify the user win the bid
  const identifyWinner = async () => {
    try {
      const usersRef = ref(db, `auctions/${realEstateId}/users`);

      // Get all users and their bids
      const usersSnapshot = await get(usersRef);
      const usersData = usersSnapshot.val();
      const userId = localStorage.getItem("userId");

      let highestBid = 0;
      let winningUserId = 0;
      const userList: userHistory[] = [];
      // Iterate over users to find the highest bid and winning user
      if (auction) {
        if (Object.keys(usersData).length === 1) {
          const userId = Object.keys(usersData)[0];
          const userBid = auction.floorBid;
          highestBid = userBid; // Set highestBid as user's bid
          winningUserId = parseInt(userId, 10); // Set winningUserId as user's ID
          userList.push({
            accountId: parseInt(usersData[userId].userId, 10),
            lastBidAmount: auction.floorBid,
          });
        } else {
          // Iterate over users to find the highest bid and winning user
          Object.keys(usersData).forEach((userId) => {
            userList.push({
              accountId: parseInt(usersData[userId].userId, 10),
              lastBidAmount: usersData[userId].currentUserBid,
            });
            const userBid = usersData[userId].currentUserBid;
            if (userBid >= highestBid) {
              highestBid = userBid;
              winningUserId = parseInt(userId, 10);
            }
          });
        }
      }

      if (userId) {
        console.log(userId);
        console.log(winningUserId);
        if (userId == winningUserId.toString()) {
          if (auction) {
            const auctionDetailDto = {
              auctionId: auction.auctionId,
              accountWinId: winningUserId,
              winAmount: highestBid,
            };
            if (token) {
              await auctionSuccess(auctionDetailDto, userList, token);
            }
          }
        }
      }
    } catch (error) {
      console.error("Error identifying winner:", error);
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

  // const showModal = () => {
  //   setIsModalOpen(true);
  // };

  // const handleOk = () => {
  //   setIsModalOpen(false);
  // };

  // const handleCancel = () => {
  //   setIsModalOpen(false);
  // };

  // const handleDecrease = () => {
  //   setCurrentAutoBidValue(currentAutoBidValue - 1000); // Decrease currentBid by 1
  // };

  // const handleIncrease = () => {
  //   setCurrentAutoBidValue(currentAutoBidValue + 1000); // Increase currentBid by 1
  // };

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
      const userId = localStorage.getItem("userId");
      // Update the currentBid attribute in Firebase
      const auctionRef = ref(db, `auctions/${auction?.reasId}`);
      await update(auctionRef, {
        currentBid: currentInputBid,
        lastBid: Date.now(),
        isBidded: 1,
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

  //subcribe for the new bid
  useEffect(() => {
    const currentBidRef = ref(db, `auctions/${realEstateId}/currentBid`);
    const unsubscribe = onValue(currentBidRef, (snapshot) => {
      const newBid = snapshot.val();
      setCurrentBid(newBid);
      setCurrentInputBid(newBid);
    });

    return () => {
      unsubscribe();
    };
  }, [realEstateId]);

  //subcribe the user registered join the auction
  useEffect(() => {
    const usersRef = ref(db, `auctions/${realEstateId}/users`);
    const unsubscribe = onValue(usersRef, (snapshot) => {
      let usersList = 0;
      snapshot.forEach(() => {
        usersList++;
      });
      setTotalUsers(usersList);
    });
    return () => {
      unsubscribe();
    };
  }, [realEstateId]);

  //subcribe the status of the auction
  useEffect(() => {
    const statusRef = ref(db, `auctions/${realEstateId}/status`);
    const unsubscribe = onValue(statusRef, (snapshot) => {
      const statusValue = snapshot.val();
      if (statusValue == 0) {
        setIsUserRegistered(false);
        setIsAuctionOpen(false);
        setIsOneParticipant(false);
        setIsUserAttend(false);
      } else if (statusValue == 1) {
        setIsUserRegistered(true);
        setIsOneParticipant(true);
      } else if (statusValue == 2) {
        setIsUserRegistered(true);
        setIsOneParticipant(false);
        setIsUserAttend(false);
      } else if (statusValue == 3) {
        setIsOneParticipant(false);
        setIsUserAttend(true);
        setIsAuctionOpen(true);
        setIsUserRegistered(true);
      } else if (statusValue == 4) {
        setIsUserRegistered(true);
        setIsAuctionOpen(false);
        setIsOneParticipant(false);
        setIsUserAttend(true);
        setIsAuctionEnd(true);
        identifyWinner();
      }
    });
    return () => {
      unsubscribe();
    };
  }, [realEstateId]);

  useEffect(() => {
    const statusRef = ref(db, `auctions/${realEstateId}/isBidded`);
    const unsubscribe = onValue(statusRef, (snapshot) => {
      const statusValue = snapshot.val();
      if (statusValue == 0) {
        return;
      } else if (statusValue == 1) {
        const userList: userHistory[] = [];
        const usersRef = ref(db, `auctions/${realEstateId}/users`);

        // Get all users and their bids
        const StartTheAuction = async () => {
          const usersSnapshot = await get(usersRef);
          const usersData = usersSnapshot.val();

          if (auction && token) {
            const auctionDetailDto = {
              auctionId: auction.auctionId,
              accountWinId: 0,
              winAmount: 0,
            };

            Object.keys(usersData).forEach((userId) => {
              userList.push({
                accountId: parseInt(usersData[userId].userId, 10),
                lastBidAmount: usersData[userId].currentUserBid,
              });
            });
            StartAuction(auctionDetailDto, userList, token);
          }
        };
        StartTheAuction();
      }
    });
    return () => {
      unsubscribe();
    };
  }, [realEstateId]);

  const handleOnFinish = () => {
    const auctionRef = ref(db, `auctions/${realEstateId}`);
    update(auctionRef, {
      status: 4,
    });
    setIsAuctionOpen(false);
    setIsAuctionEnd(true);
  };

  const handleOnFinishWaiting10Mins = () => {
    const auctionRef = ref(db, `auctions/${realEstateId}`);
    update(auctionRef, {
      status: 3,
    });
  };

  // const toggleChecked = () => {
  //   if (checked == true) {
  //     setChecked(!checked);
  //   } else {
  //     if (currentAutoBidValue > 0) {
  //       setIsInputValid(true); // Set validation state to true if input value is larger than 0
  //       setChecked(!checked);
  //     } else {
  //       setIsInputValid(false); // Set validation state to false otherwise
  //     }
  //   }
  // };

  const handleRegister = () => {
    try {
      const fetchPaymentUrl = async () => {
        if (userId && token) {
          if (realEstateDetail?.reasId) {
            const response = await registerParticipateAuction(
              userId,
              realEstateDetail?.reasId,
              token
            );
            if (response) {
              const depositId = response?.depositAmountDto.depositId;
              console.log(depositId);
              getDeposit(depositId);
              window.location.href = response?.paymentUrl;
            }
          }
        }
      };
      fetchPaymentUrl();
    } catch (error) {
      console.log("Error:", error);
    }
  };

  const handleCountDownStarting = () => {
    setAuctionStatus(5);
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
                {userId === realEstateDetail?.accountOwnerId ? (
                  <li>
                    <button
                      className={getActiveTab("ownership")}
                      onClick={() => toggleTab("ownership")}
                    >
                      Ownership
                    </button>
                  </li>
                ) : (
                  <></>
                )}
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
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      d="M8 17.3a5 5 0 0 0 2.6 1.7c2.2.6 4.5-.5 5-2.3.4-2-1.3-4-3.6-4.5-2.3-.6-4-2.7-3.5-4.5.5-1.9 2.7-3 5-2.3 1 .2 1.8.8 2.5 1.6m-3.9 12v2m0-18v2.2"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.reasPrice
                      ? NumberFormat(realEstateDetail?.reasPrice)
                      : realEstateDetail?.reasPrice}{" "}
                    VND
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
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      d="M9 7H7m2 3H7m2 3H7m4 2v2m3-2v2m3-2v2M4 5v14c0 .6.4 1 1 1h14c.6 0 1-.4 1-1v-3c0-.6-.4-1-1-1h-9a1 1 0 0 1-1-1V5c0-.6-.4-1-1-1H5a1 1 0 0 0-1 1Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.reasArea}
                  </div>
                  <div className="text-xs text-gray-700">m²</div>
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
                      strokeWidth="2"
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
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
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
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
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
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
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
              auction ? (
                auctionStatus === 0 ? (
                  <div className="flex justify-center p-10">
                    <div className="text-4xl text-red-700">
                      Real Estate Is Currently Not Available For Selling
                    </div>
                  </div>
                ) : auctionStatus === 1 ? (
                  <div className="flex justify-center flex-col items-center py-10">
                    <div className="text-xl text-gray-500">
                      You have not register to take part in auction yet
                    </div>
                    <button
                      onClick={() => handleRegister()}
                      className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                    >
                      Register
                    </button>
                  </div>
                ) : auctionStatus === 2 ? (
                  <div className="flex justify-center flex-col items-center py-10">
                    <div className="text-xl text-gray-500">
                      Auction register is pending
                    </div>
                    <button
                      onClick={() => handleRegister()}
                      className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                    >
                      Register
                    </button>
                  </div>
                ) : auctionStatus === 3 ? (
                  <div className="flex justify-center p-10">
                    <div className="text-4xl text-blue-700">
                      Register auction success
                    </div>
                  </div>
                ) : auctionStatus === 4 ? (
                  <div className="flex justify-center p-10">
                    <div className="text-4xl text-red-700">
                      You Are The Owner of This Real Estate
                    </div>
                  </div>
                ) : auctionStatus === 5 ? (
                  <>
                    <div>
                      <Typography variant="h3" className="text-center">
                        Current bid: {NumberFormat(currentBid)}
                      </Typography>
                    </div>

                    <div className="grid grid-cols-5 gap-4">
                      <div className="col-span-3">
                        <Typography variant="h5">
                          Auction start:{" "}
                          {dayjs(auction?.dateStart).format(
                            "DD/MM/YYYY HH:mm:ss"
                          )}
                        </Typography>
                        <div className="font-semibold flex items-center">
                          Total user: {totalUsers}
                        </div>
                        {!isAuctionEnd ? (
                          <Countdown
                            value={dayjs(auction.dateEnd).toDate().toString()}
                            format="H [hours] m [minutes] s [secs]"
                            prefix="Remain time"
                            onFinish={handleOnFinish}
                          />
                        ) : (
                          <Typography variant="h5">
                            Auction end:{" "}
                            {dayjs(auction?.dateEnd).format(
                              "DD/MM/YYYY HH:mm:ss"
                            )}
                          </Typography>
                        )}
                      </div>

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
                                          value={NumberFormat(currentInputBid)}
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
                              <Countdown
                                value={dayjs(auction.dateStart)
                                  .add(10, "minutes")
                                  .toString()}
                                format=" m [minutes] s [secs]"
                                onFinish={() => handleOnFinishWaiting10Mins()}
                                prefix={<>Auction will start in </>}
                              />
                            </>
                          )}
                        </>
                      ) : (
                        <>
                          <Countdown
                            value={dayjs(auction.dateStart)
                              .add(10, "minutes")
                              .toString()}
                            format=" m [minutes] s [secs]"
                            onFinish={() => handleOnFinishWaiting10Mins()}
                            prefix={<>Waiting for users in </>}
                          />
                        </>
                      )}
                    </div>
                  </>
                ) : auctionStatus === 6 ? (
                  <div className="flex justify-center p-10">
                    <div className="text-4xl text-green-700">
                      Auction Is Waiting To Be Start
                    </div>
                    <div>
                      <Countdown
                        value={auction.dateStart.toString()}
                        onFinish={handleCountDownStarting}
                      />
                    </div>
                  </div>
                ) : auctionStatus === 7 ? (
                  <div className="flex justify-center p-10">
                    <div className="text-4xl text-red-700">
                      Lost Deposit, You was late to the auction
                    </div>
                  </div>
                ) : (
                  <></>
                )
              ) : userId === realEstateDetail?.accountOwnerId ? (
                <div className="flex justify-center p-10">
                  <div className="text-4xl text-red-700">
                    You Are The Owner of This Real Estate
                  </div>
                </div>
              ) : auctionStatus === 0 ? (
                <div className="flex justify-center p-10">
                  <div className="text-4xl text-red-700">
                    Real Estate Is Currently Not Available For Selling
                  </div>
                </div>
              ) : auctionStatus === 1 ? (
                <div className="flex justify-center flex-col items-center py-10">
                  <div className="text-xl text-gray-500">
                    You have not register to take part in auction yet
                  </div>
                  <button
                    onClick={() => handleRegister()}
                    className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                  >
                    Register
                  </button>
                </div>
              ) : auctionStatus === 2 ? (
                <div className="flex justify-center flex-col items-center py-10">
                  <div className="text-xl text-gray-500">
                    Auction register is pending
                  </div>
                  <button
                    onClick={() => handleRegister()}
                    className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                  >
                    Register
                  </button>
                </div>
              ) : auctionStatus === 3 ? (
                <div className="flex justify-center p-10">
                  <div className="text-4xl text-blue-700">
                    Register auction success
                  </div>
                </div>
              ) : auctionStatus === 4 ? (
                <div className="flex justify-center p-10">
                  <div className="text-4xl text-red-700">
                    You Are The Owner of This Real Estate
                  </div>
                </div>
              ) : auctionStatus === 7 ? (
                <div className="flex justify-center p-10">
                  <div className="text-4xl text-red-700">
                    Lost Deposit, You was late to the auction
                  </div>
                </div>
              ) : (
                <div className="flex justify-center flex-col items-center py-10">
                  <div className="text-xl text-gray-500">
                    This auction is currently not created
                  </div>
                  <button
                    onClick={() => handleRegister()}
                    className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                  >
                    Register
                  </button>
                </div>
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
          <div className={getActiveTabDetail("ownership")}>
            <div className="grid md:grid-cols-3 sm:grid-cols-2 gap-2">
              <div>
                <div className="text-center text-lg">Front Certificate</div>
                <div className="flex items-center justify-center w-full h-64">
                  {realEstateDetail?.detail &&
                  realEstateDetail?.detail.reas_Cert_Of_Land_Img_Front ? (
                    <img
                      className="text-transparent w-full h-full object-fill rounded-lg"
                      src={realEstateDetail?.detail.reas_Cert_Of_Land_Img_Front}
                    />
                  ) : (
                    <div
                      className="flex justify-center items-center text-4xl text-gray-200
                    "
                    >
                      No Image
                    </div>
                  )}
                </div>
              </div>
              <div>
                <div className="text-center text-lg">Back Certificate</div>
                <div className="flex items-center justify-center w-full h-64">
                  {realEstateDetail?.detail &&
                  realEstateDetail?.detail.reas_Cert_Of_Land_Img_After ? (
                    <img
                      className="text-transparent w-full h-full object-fill rounded-lg"
                      src={realEstateDetail?.detail.reas_Cert_Of_Land_Img_After}
                    />
                  ) : (
                    <div
                      className="flex justify-center items-center text-4xl text-gray-200
                    "
                    >
                      No Image
                    </div>
                  )}
                </div>
              </div>
              <div>
                <div className="text-center text-lg">Ownership Certificate</div>
                <div className="flex items-center justify-center w-full h-64">
                  {realEstateDetail?.detail &&
                  realEstateDetail?.detail.reas_Cert_Of_Home_Ownership ? (
                    <img
                      className="text-transparent w-full h-full object-fill rounded-lg"
                      src={realEstateDetail?.detail.reas_Cert_Of_Home_Ownership}
                    />
                  ) : (
                    <div
                      className="flex justify-center items-center text-4xl text-gray-200
                    "
                    >
                      No Image
                    </div>
                  )}
                </div>
              </div>
              <div>
                <div className="text-center text-lg">Registration Book</div>
                <div className="flex items-center justify-center w-full h-64">
                  {realEstateDetail?.detail &&
                  realEstateDetail?.detail.reas_Registration_Book ? (
                    <img
                      className="text-transparent w-full h-full object-fill rounded-lg"
                      src={realEstateDetail?.detail.reas_Registration_Book}
                    />
                  ) : (
                    <div
                      className="flex justify-center items-center text-4xl text-gray-200
                    "
                    >
                      No Image
                    </div>
                  )}
                </div>
              </div>
              <div>
                <div className="text-center text-lg">Relationship Document</div>
                <div className="flex items-center justify-center w-full h-64">
                  {realEstateDetail?.detail &&
                  realEstateDetail?.detail
                    .documents_Proving_Marital_Relationship ? (
                    <img
                      className="text-transparent w-full h-full object-fill rounded-lg"
                      src={
                        realEstateDetail?.detail
                          .documents_Proving_Marital_Relationship
                      }
                    />
                  ) : (
                    <div
                      className="flex justify-center items-center text-4xl text-gray-200
                    "
                    >
                      No Image
                    </div>
                  )}
                </div>
              </div>
              <div>
                <div className="text-center text-lg">
                  Authorization Contract
                </div>
                <div className="flex items-center justify-center w-full h-64">
                  {realEstateDetail?.detail &&
                  realEstateDetail?.detail.sales_Authorization_Contract ? (
                    <img
                      className="text-transparent w-full h-full object-fill rounded-lg"
                      src={
                        realEstateDetail?.detail.sales_Authorization_Contract
                      }
                    />
                  ) : (
                    <div
                      className="flex justify-center items-center text-4xl text-gray-200
                    "
                    >
                      No Image
                    </div>
                  )}
                </div>
              </div>
            </div>
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
