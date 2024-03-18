import { useContext, useEffect, useState } from "react";
import realEstate from "../../interface/RealEstate/realEstate";
import { UserContext } from "../../context/userContext";
import { payRealEstatePostingFee } from "../../api/transaction";
import { ReasContext } from "../../context/reasContext";
import { Tag } from "antd";
import { Link } from "react-router-dom";
interface RealEstateProps {
  realEstate: realEstate;
  ownRealEstatesStatus?: boolean;
}

const statusAllColorMap: { [key: string]: string } = {
  InProgress: "green",
  Approved: "green",
  Selling: "orange",
  Cancel: "red",
  Auctioning: "lightgreen",
  Sold: "brown",
  Rollback: "red",
  DeclineAfterAuction: "darkred",
  Success: "lightcoral",
};

const RealEstateCard = ({
  realEstate,
  ownRealEstatesStatus,
}: RealEstateProps) => {
  const [estate, setEstate] = useState<realEstate | undefined>(realEstate);
  const [formattedDateEnd, setFormattedDateEnd] = useState<string>("");
  const [showStatus, setShowStatus] = useState<boolean>();
  const { token, userId } = useContext(UserContext);
  const { getReas } = useContext(ReasContext);

  useEffect(() => {
    setEstate(realEstate || undefined);
    if (realEstate?.dateEnd) {
      const dateObject = new Date(realEstate.dateEnd);
      const formattedDate = dateObject
        .toDateString()
        .split(" ")
        .slice(1)
        .join(" ");
      setFormattedDateEnd(formattedDate);
    }
  }, []);

  useEffect(() => {
    setShowStatus(ownRealEstatesStatus);
  }, [ownRealEstatesStatus]);

  function formatVietnameseDong(price: string) {
    // Convert the string to a number
    const numberPrice = parseInt(price, 10);
    // Check if the conversion was successful
    if (isNaN(numberPrice)) {
      // Return the original string if it's not a valid number
      return price;
    }
    // Format the number
    const formattedNumber = numberPrice
      .toString()
      .replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    return formattedNumber;
  }

  const handlePayingFee = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    e.preventDefault();
    e.stopPropagation();
    try {
      const fetchPaymentUrl = async () => {
        if (userId && token) {
          if (estate?.reasId) {
            const response = await payRealEstatePostingFee(
              userId,
              estate?.reasId,
              token
            );
            if (response) {
              getReas(estate?.reasId);
              window.location.href = response?.paymentUrl as string;
            }
          }
        }
      };
      fetchPaymentUrl();
    } catch (error) {
      console.log("Error:", error);
    }
  };

  const handleUpdateButtonClick = (
    e: React.MouseEvent<HTMLAnchorElement, MouseEvent>
  ) => {
    e.stopPropagation(); // Stop event propagation here
  };

  return (
    <div className="max-w-sm bg-white border border-gray-200 rounded-lg mx-auto sm:my-2 md:my-0 shadow-lg hover:shadow-xl transition-all delay-100">
      <div className="">
        <img
          className="rounded-t-lg h-52 w-full"
          src={estate?.uriPhotoFirst}
          alt=""
        />
      </div>
      <div className="p-5">
        <div>
          <h5 className="mb-2 text-xl font-bold tracking-tight text-gray-900 xl:line-clamp-2 md:line-clamp-3 break-all">
            {estate?.reasName}
          </h5>
        </div>
        <div className="mb-3 font-normal text-gray-700">
          <span className="text-gray-900 font-semibold">
            {estate?.reasTypeName}
          </span>
          <span className="sm:inline md:hidden xl:inline"> | </span>
          <br className="sm:hidden md:block xl:hidden" />
          <span className="text-gray-900 font-semibold">
            {estate?.reasArea}
          </span>
          <span> (m²)</span>
        </div>

        <div className="flex text-gray-700">
          <div className="text-xl font-bold tracking-tight text-gray-900 ">
            {estate?.reasPrice
              ? formatVietnameseDong(estate?.reasPrice)
              : estate?.reasPrice}
            <span className="pl-1">VND</span>
          </div>
        </div>
        <div
          className={`${
            showStatus ? "justify-between" : "justify-end"
          } flex  text-gray-700`}
        >
          <div>
            {showStatus ? (
              <Tag color={statusAllColorMap[estate?.reasStatus || ""]}>
                {estate?.reasStatus}
              </Tag>
            ) : (
              <></>
            )}
          </div>
          <div className=" tracking-tight">
            Due:{" "}
            <span className="text-gray-900 font-semibold">
              {formattedDateEnd}
            </span>
          </div>
        </div>
        <div className="flex justify-center mt-1">
          {showStatus ? (
            estate?.reasStatus === "Approved" ? (
              <button
                onClick={handlePayingFee}
                className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
              >
                Pay Posting Fee
              </button>
            ) : estate?.reasStatus === "Rollback" ||
              estate?.reasStatus === "DeclineAfterAuction" ? (
              <button
                className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center"
                onClick={() => handleUpdateButtonClick}
              >
                <Link to={`/update/${estate?.reasId}`}>Update</Link>
              </button>
            ) : (
              <div className="text-transparent bg-transparent text-sm px-4 py-2 ">
                .
              </div>
            )
          ) : (
            <></>
          )}
        </div>
      </div>
    </div>
  );
};

export default RealEstateCard;
