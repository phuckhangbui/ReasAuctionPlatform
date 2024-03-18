import { useEffect, useState } from "react";

interface AuctionProps {
  auction: auction;
}

const AuctionCard = ({ auction }: AuctionProps) => {
  const [auctionDetail, setAuctionDetail] = useState<auction | undefined>(
    auction
  );
  const [formattedDateEnd, setFormattedDateEnd] = useState<string>("");

  useEffect(() => {
    setAuctionDetail(auction || undefined);
    if (auction?.dateStart) {
      const dateObject = new Date(auction.dateStart);
      const formattedDate = dateObject
        .toDateString()
        .split(" ")
        .slice(1)
        .join(" ");
      setFormattedDateEnd(formattedDate);
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
        <p className="mb-3 text-mainBlue text-xl font-bold">00:00:00:00</p>
        <div className="flex justify-between items-center">
          <div className=" font-bold tracking-tight text-gray-900 ">
            {/* <span className="font-normal">Current Bid:</span> ${realEstate.price} */}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuctionCard;
