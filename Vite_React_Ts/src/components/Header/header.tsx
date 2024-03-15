import React, { useContext, useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import LoginModal from "../LoginModal/loginModal";
import { UserContext } from "../../context/userContext";
import { AvatarDropdown } from "../AvatarDropdown/AvatarDropdown";

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const currentUrl = useLocation();
  const [showModal, setShowModal] = useState(false);
  const { userRole } = useContext(UserContext);

  const getActiveLink = (url: string) => {
    return `${
      currentUrl.pathname.includes(url)
        ? "text-white bg-mainBlue rounded md:bg-transparent md:text-mainBlue"
        : "text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-mainBlue"
    } block py-2 px-3 md:p-0`;
  };

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const closeModal = () => {
    setShowModal(!showModal);
  };

  const toggleModal = () => {
    setShowModal((prevShowModal) => !prevShowModal);
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
            <>
              {/* <span className="text-mainBlue">{user.username}</span>
              <button
                onClick={() => logout()} // Call logout function from UserContext
                className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
              >
                Logout
              </button> */}
              <AvatarDropdown />
            </>
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
            {/* <li>
              <Link to={"/help"} className={getActiveLink("help")}>
                Help
              </Link>
            </li> */}
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
    </nav>
  );
};

export default Header;
