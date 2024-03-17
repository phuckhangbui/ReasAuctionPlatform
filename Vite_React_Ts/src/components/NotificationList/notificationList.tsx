import { useState } from "react";

const NotificationList = () => {
  return (
    <>
      <div className="block px-4 py-2 font-medium text-center text-gray-700 rounded-t-lg bg-gray-100 ">
        Notifications
      </div>
      <div className="divide-y divide-gray-100 dark:divide-gray-700">
        <a
          href="#"
          className="flex px-4 py-3 hover:bg-gray-100 dark:hover:bg-gray-700"
        >
          <div className="flex-shrink-0">
            <img
              className="rounded-full w-11 h-11"
              src="/docs/images/people/profile-picture-4.jpg"
              alt="Leslie image"
            />
            <div className="absolute flex items-center justify-center w-5 h-5 ms-6 -mt-5 bg-green-400 border border-white rounded-full dark:border-gray-800">
              <svg
                className="w-2 h-2 text-white"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="currentColor"
                viewBox="0 0 20 18"
              >
                <path d="M18 0H2a2 2 0 0 0-2 2v9a2 2 0 0 0 2 2h2v4a1 1 0 0 0 1.707.707L10.414 13H18a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2Zm-5 4h2a1 1 0 1 1 0 2h-2a1 1 0 1 1 0-2ZM5 4h5a1 1 0 1 1 0 2H5a1 1 0 0 1 0-2Zm2 5H5a1 1 0 0 1 0-2h2a1 1 0 0 1 0 2Zm9 0h-6a1 1 0 0 1 0-2h6a1 1 0 1 1 0 2Z" />
              </svg>
            </div>
          </div>
          <div className="w-full ps-3">
            <div className="text-gray-500 text-sm mb-1.5 dark:text-gray-400">
              <span className="font-semibold text-gray-900 dark:text-white">
                Leslie Livingston
              </span>{" "}
              mentioned you in a comment:{" "}
              <span className="font-medium text-blue-500">@bonnie.green</span>{" "}
              what do you say?
            </div>
            <div className="text-xs text-blue-600 dark:text-blue-500">
              1 hour ago
            </div>
          </div>
        </a>
      </div>
      <a
        href="#"
        className="block py-2 text-sm font-medium text-center text-gray-900 rounded-b-lg bg-gray-50 hover:bg-gray-100"
      >
        <div className="inline-flex items-center ">
          <svg
            className="w-4 h-4 me-2 text-gray-500 dark:text-gray-400"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="currentColor"
            viewBox="0 0 20 14"
          >
            <path d="M10 0C4.612 0 0 5.336 0 7c0 1.742 3.546 7 10 7 6.454 0 10-5.258 10-7 0-1.664-4.612-7-10-7Zm0 10a3 3 0 1 1 0-6 3 3 0 0 1 0 6Z" />
          </svg>
          View all
        </div>
      </a>
    </>
  );
};

export default NotificationList;
