import { useEffect, useState } from "react";

interface AuctionProps {
  auction: auction;
}

const AuctionCard = ({ auction }: AuctionProps) => {
  const [auctionDetail, setAuctionDetail] = useState<auction | undefined>(
    auction
  );
  const [formattedDateStart, setFormattedDateStart] = useState<string>("");
  const [formattedTimeStart, setFormattedTimeStart] = useState<string>("");

  useEffect(() => {
    setAuctionDetail(auction || undefined);
    if (auction?.dateStart) {
      const dateObject = new Date(auction.dateStart);
      const formattedDate = dateObject
        .toDateString()
        .split(" ")
        .slice(1)
        .join(" ");
      setFormattedDateStart(formattedDate);

      const hours = dateObject.getHours();
      const minutes = dateObject.getMinutes();

      const formattedTime = `${hours}:${
        minutes < 10 ? "0" + minutes : minutes
      }`;
      setFormattedTimeStart(formattedTime);
    }
  }, []);

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

  return (
    <div className="max-w-sm bg-white border border-gray-200 rounded-lg mx-auto sm:my-2 md:my-0 shadow-lg hover:shadow-xl transition-all delay-100">
      <div className="">
        <img
          className="rounded-t-lg h-52 w-full"
          src={auctionDetail?.thumbnailUrl}
          alt=""
        />
      </div>
      <div className="p-5">
        <div>
          <h5 className="mb-2 text-xl font-bold tracking-tight text-gray-900 xl:line-clamp-2 md:line-clamp-3 break-all">
            {auctionDetail?.reasName}
          </h5>
        </div>
        <p className="mb-3  text-xl font-bold">
          Floor Bid:{" "}
          <span className="text-mainBlue">
            {auctionDetail?.floorBid
              ? formatVietnameseDong(auctionDetail?.floorBid)
              : auctionDetail?.floorBid}{" "}
            VND
          </span>
        </p>
        <div className="flex justify-between items-center">
          <div className=" font-bold tracking-tight text-gray-900 ">
            {/* <span className="font-normal">Current Bid:</span> ${realEstate.price} */}
          </div>
        </div>
        <div className="md:flex-col 2xl:flex-row xl:flex sm:flex lg:justify-between sm:flex-row sm:justify-between">
          <div className=" tracking-tight flex justify-end">
            Time Start:{" "}
            <span className="text-gray-900 font-semibold pl-1">
              {formattedTimeStart && `${formattedTimeStart}`}
            </span>
          </div>
          <div className=" tracking-tight flex justify-end">
            Date:{" "}
            <span className="text-gray-900 font-semibold pl-1">
              {formattedDateStart}
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuctionCard;
