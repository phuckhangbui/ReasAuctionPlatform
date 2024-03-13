import { useContext, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { DepositContext } from "../../../context/depositContext";
import { UserContext } from "../../../context/userContext";
import { payDeposit } from "../../../api/transaction";
import { ReasContext } from "../../../context/reasContext";

const SuccessPage = () => {
  const { depositId, removeDeposit } = useContext(DepositContext);
  const { reasId, removeReas } = useContext(ReasContext);
  const { token } = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    let homeTimeout: NodeJS.Timeout;
    try {
      console.log(window.location.href);
      const url = window.location.href;
      const queryString = url.substring(url.indexOf("?") + 1);
      // const params: Record<string, string> = queryString.split('&').reduce((acc: Record<string, string>, param) => {
      //   const [key, value] = param.split('=');
      //   acc[key] = decodeURIComponent(value);
      //   return acc;
      // }, {});
      // () => navigate("/")
      console.log(queryString);
      const postPayment = async () => {
        if (token) {
          let response;
          if (depositId && reasId == undefined) {
            response = await payDeposit(queryString, depositId, token);
          } else if (reasId && depositId == undefined) {
            // response = await
          }
          if (response) {
            removeDeposit();
            removeReas();
            homeTimeout = setTimeout(() => navigate("/"), 2000);
          }
        }
      };
      postPayment();
    } catch (error) {
      console.log("Error:", error);
    }
    return () => clearTimeout(homeTimeout);
  }, []);
  return (
    <div className="flex flex-col justify-center items-center h-75vh w-full text-center pt-30">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
        stroke-width="1.5"
        stroke="currentColor"
        className="w-28 h-28 bg-mainBlue rounded-full p-2 text-white"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="m4.5 12.75 6 6 9-13.5"
        />
      </svg>
      <div className="text-sm pt-3 pb-1">SUCCESS</div>
      <div className="font-bold text-2xl pb-2">Transaction Completed!</div>
      <div className="text-gray-700 pb-3">
        The transaction is completed, thank for choosing our services.
      </div>
      <div className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
        <Link to="/">Continue Browsing</Link>
      </div>
    </div>
  );
};

export default SuccessPage;
