import React, { useContext, useEffect, useRef, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import LoginModal from "../LoginModal/loginModal";
import { UserContext } from "../../context/userContext";
import { AvatarDropdown } from "../AvatarDropdown/AvatarDropdown";
import NotificationList from "../NotificationList/notificationList";
import { getNotification } from "../../api/member";
import { NotificationContext } from "../../context/notificationContext";

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [isNotiOpen, setIsNotiOpen] = useState(false);
  const [notificationList, setNotificationList] = useState<
    notification[] | undefined
  >([]);
  const currentUrl = useLocation();
  const { userRole, token } = useContext(UserContext);
  const { hasNewNoti, updateNotiStatus } = useContext(NotificationContext);
  const notificationRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    try {
      const fetchNotificationList = async () => {
        if (token) {
          const response = await getNotification(token);
          if (response) {
            setNotificationList(response);
            updateNotiStatus(false);
          }
        }
      };
      fetchNotificationList();
    } catch (error) {
      console.log("Error:", error);
    }
  }, [hasNewNoti, token]);

  const getActiveLink = (url: string) => {
    return `${
      currentUrl.pathname.includes(url)
        ? "text-white bg-mainBlue rounded md:bg-transparent md:text-mainBlue"
        : "text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-mainBlue"
    } block py-2 px-3 md:p-0`;
  };

  useEffect(() => {
    console.log("isNotiOpe:", isNotiOpen);
  }, [isNotiOpen]);

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const closeModal = () => {
    setShowModal(!showModal);
  };

  const toggleModal = () => {
    setShowModal((prevShowModal) => !prevShowModal);
  };

  const toggleNotiList = () => {
    setIsNotiOpen(!isNotiOpen);
  };
  const closeNotiList = () => {
    setIsNotiOpen(false);
  };

  const handleOverlayClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) {
      // If the click occurs on the overlay (not on the modal content), close the modal
      closeModal();
    }
  };

  useEffect(() => {
    // Disable scroll on body when modal is open
    if (showModal) {
      document.body.style.overflow = "hidden";
    } else {
      document.body.style.overflow = "auto";
    }
    // Cleanup function
    return () => {
      document.body.style.overflow = "auto";
    };
  }, [showModal]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (notificationRef.current && !notificationRef.current.contains(event.target as Node)) {
        // Click occurred outside of the notification list, so close it
        closeNotiList();
      }
    };
    // Add event listener when component mounts
    document.addEventListener("mousedown", handleClickOutside);
    
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
      document.body.style.overflow = "auto";
    };
  },[isNotiOpen])

  return (
    <nav className="bg-white fixed w-full top-0 start-0 border-gray-200 z-10">
      <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
        <Link
          to={"/"}
          className="flex items-center space-x-3 rtl:space-x-reverse"
        >
          <img
            src="./REAS-removebg-preview.png"
            className="h-8"
            alt="Flowbite Logo"
          />
          <span className="self-center text-2xl font-semibold whitespace-nowrap text-mainBlue">
            REAS
          </span>
        </Link>
        <div className="flex md:order-2 space-x-3 md:space-x-0 rtl:space-x-reverse">
          {userRole ? (
            <div className="flex items-center">
              <button
                onClick={toggleNotiList}
                type="button"
                className="mr-3"
                aria-expanded={isNotiOpen}
              >
                <span className="sr-only">Open Notification</span>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth="1.5"
                  stroke="currentColor"
                  className="w-6 h-6"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M14.857 17.082a23.848 23.848 0 0 0 5.454-1.31A8.967 8.967 0 0 1 18 9.75V9A6 6 0 0 0 6 9v.75a8.967 8.967 0 0 1-2.312 6.022c1.733.64 3.56 1.085 5.455 1.31m5.714 0a24.255 24.255 0 0 1-5.714 0m5.714 0a3 3 0 1 1-5.714 0"
                  />
                </svg>
              </button>
              <AvatarDropdown />
            </div>
          ) : (
            <button
              onClick={() => toggleModal()}
              className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
            >
              Sign In
            </button>
          )}

          <button
            onClick={toggleMenu}
            type="button"
            className="inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600"
            aria-expanded={isMenuOpen}
          >
            <span className="sr-only">Open main menu</span>
            <svg
              className="w-5 h-5"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 17 14"
            >
              <path
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M1 1h15M1 7h15M1 13h15"
              />
            </svg>
          </button>
        </div>
        <div
          className={`${
            isMenuOpen ? "block" : "hidden"
          } items-center justify-between w-full md:flex md:w-auto md:order-1`}
          id="navbar-cta"
        >
          <ul className="flex flex-col font-medium p-4 md:p-0 mt-4 border border-gray-100 rounded-lg bg-gray-50 md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0 md:bg-white">
            <li>
              <Link to={"/realEstate"} className={getActiveLink("realEstate")}>
                Real Estate
              </Link>
            </li>
            <li>
              <Link to={"/auction"} className={getActiveLink("auction")}>
                Auction
              </Link>
            </li>
            <li>
              <Link to={"/news"} className={getActiveLink("news")}>
                News
              </Link>
            </li>
            <li>
              {userRole === 3 ? (
                <Link to={"/sell"} className={getActiveLink("sell")}>
                  Sell
                </Link>
              ) : (
                <></>
              )}
            </li>
          </ul>
        </div>
      </div>
      {showModal && (
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
      <div
        id="dropdownNotification"
        className={`${
          isNotiOpen ? "block" : "hidden"
        } z-50 w-full max-w-sm bg-white divide-y divide-gray-100 absolute flex right-3 transition-all duration-300 ease-in rounded-lg `}
        aria-labelledby="dropdownNotificationButton"
        ref={notificationRef}
      >
        <div className="rounded-lg shadow ">
          <NotificationList notificationList={notificationList} />
        </div>
      </div>
    </nav>
  );
};

export default Header;
