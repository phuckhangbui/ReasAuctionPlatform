import { useContext, useEffect, useState } from "react";
import realEstate from "../../interface/RealEstate/realEstate";
import { UserContext } from "../../context/userContext";
import { payRealEstatePostingFee } from "../../api/transaction";
import { NumberFormat } from "../../Utils/numbetFormat";
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


  const handlePayingFee = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    try {
      e.preventDefault();
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

  return (
    <div className="max-w-2lg bg-white border border-gray-200 rounded-lg shadow mx-auto sm:my-2 md:my-0">
      {showStatus && (
        <div className="flex justify-end pt-2 pr-2">
          {estate?.reasStatus === "Approved" ? (
            <Tag color="green">Approved</Tag>
          ) : (
            <Tag color={statusAllColorMap[estate?.reasStatus || ""]}>
              {estate?.reasStatus}
            </Tag>
          )}
        </div>
        )}
        <div className="grid grid-cols-1 lg:grid-cols-3 md:gap-3">
          <div className="lg:col-span-1">
            <img
              className="rounded-t-lg lg:h-52 lg:w-96 lg:pl-4 lg:pr-4 md:pt-4 md:w-full md:pl-20 md:pr-20 md:h-69"
              src={estate?.uriPhotoFirst}
              alt=""
            />
          </div>
          <div className="lg:col-span-2 p-5">
          <div>
            <h5 className="mb-2 text-xl font-bold tracking-tight text-gray-900 xl:line-clamp-2 md:line-clamp-3">
              {estate?.reasName}
            </h5>
          </div><div className="mb-3 font-normal text-gray-700">
            <span className="text-gray-900 font-semibold">
              {estate?.reasTypeName}
            </span>
            <span className="sm:inline md:hidden xl:inline"> | </span>
            <br className="sm:hidden md:block xl:hidden" />
            <span className="text-gray-900 font-semibold">
            {estate?.reasArea}
            </span>
            <span> sqrt</span>
          </div>
          <div className="flex text-gray-700">
            <div className="text-xl font-bold tracking-tight text-gray-900 ">
              {estate?.reasPrice
                ? NumberFormat(estate?.reasPrice)
                : estate?.reasPrice}
              <span className="pl-1">VNĐ</span>
            </div>
            </div>
          <div className="flex justify-end text-gray-700">
            <div className="tracking-tight">
              Due:{" "}
              <span className="text-gray-900 font-semibold">
                {formattedDateEnd}
              </span>
            </div>
          </div>
          {showStatus && (
            <div className="flex justify-end">
              {estate?.reasStatus === "Approved" && (
                <button
                  onClick={handlePayingFee}
                  className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 ml-2"
                >
                  Pay Posting Fee
                </button>
              )}
              {(estate?.reasStatus === "Rollback" ||
              estate?.reasStatus === "DeclineAfterAuction") && estate?.flag === false  && (
              <button
                className="text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
              >
                <Link to={`/update/${estate?.reasId}`}>Update</Link>
              </button>
            )}
            </div>
            )}
            </div>
          </div>
          <div className="flex justify-end pb-6"></div>
        </div>
      );
    };

export default RealEstateCard;
