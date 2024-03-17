import { useContext, useEffect, useState } from "react";
import realEstate from "../../interface/RealEstate/realEstate";
import { UserContext } from "../../context/userContext";
import { payRealEstatePostingFee } from "../../api/transaction";
import {reUpRealEstate} from "../../api/realEstate"
import { ReasContext } from "../../context/reasContext";
import { Tag, Button,
  Modal,
  notification, DatePicker
} from "antd";

interface RealEstateProps {
  realEstate: realEstate;
  ownRealEstatesStatus?: boolean;
}

const RealEstateCard = ({
  realEstate,
  ownRealEstatesStatus,
}: RealEstateProps) => {
  const [estate, setEstate] = useState<realEstate | undefined>(realEstate);
  const [formattedDateEnd, setFormattedDateEnd] = useState<string>("");
  const [showStatus, setShowStatus] = useState<boolean>(false); // Chỉnh sửa giá trị ban đầu của showStatus thành false
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
  }, [realEstate]);

  useEffect(() => {
    setShowStatus(ownRealEstatesStatus || false); // Chỉnh sửa giá trị ban đầu của showStatus thành false
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

  const statusAllColorMap: { [key: string]: string } = {
    "InProgress": "green",
    "Approved": "green",
    "Selling": "orange",
    "Cancel": "red",
    "Auctioning": "lightgreen",
    "Sold": "brown",
    "Rollback": "red",
    "DeclineAfterAuction": "darkred",
    "Success": "lightcoral",
  };

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

  const [isModalOpen, setIsModalOpen] = useState(false);
  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleOk = () => {
    setIsModalOpen(false);
    handleReup();
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  let dateend: Date = new Date();
  const onChangeDate = (date: any) => {
    dateend = date;
  };

  const openNotificationWithIcon = (
    type: "success" | "error",
    description: string
  ) => {
    notification[type]({
      message: "Notification Title",
      description: description,
    });
  };

  const handleReup = async () => {
    const reUp: ReupRealEstate = {
      reasId: estate?.reasId,
      dateEnd: dateend,
    };
    const response = await reUpRealEstate(reUp, token);
    if (response != undefined && response) {
      if (response.statusCode == "MSG29") {
        openNotificationWithIcon("success", response.message);
      } 
      else {
        openNotificationWithIcon(
          "error",
          "Something went wrong when executing operation. Please try again!"
        );
      }
    }
  };

  return (
    <div className="max-w-2lg bg-white border border-gray-200 rounded-lg shadow mx-auto sm:my-2 md:my-0">
      {showStatus && (
        <div className="flex justify-end pt-2 pr-2 pb-2">
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
            className="rounded-t-lg h-52 w-full"
            src={estate?.uriPhotoFirst}
            alt=""
          />
        </div>
        <div className="lg:col-span-2 p-5">
          <div>
            <h5 className="mb-2 text-xl font-bold tracking-tight text-gray-900 xl:line-clamp-2 md:line-clamp-3">
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
            <span> sqrt</span>
          </div>

          <div className="flex text-gray-700">
            <div className="text-xl font-bold tracking-tight text-gray-900 ">
              {estate?.reasPrice
                ? formatVietnameseDong(estate?.reasPrice)
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
                estate?.reasStatus === "DeclineAfterAuction") && (
                  <div>
                  <Button onClick={showModal}>Re-up</Button>
                  <Modal
                    title="Fill information to re-up Reas Estate"
                    open={isModalOpen}
                    onOk={handleOk}
                    onCancel={handleCancel}
                    footer={[
                      <Button key="submit" onClick={handleOk}>
                        Re-up
                      </Button>,
                    ]}
                  >
                    <div style={{ alignContent: "center" }}>
                      <p><strong>Date End :</strong></p>
                <DatePicker
                  onChange={onChangeDate}
                  needConfirm={false}
                />
              </div>
              <br />
              </Modal>
                  </div>
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