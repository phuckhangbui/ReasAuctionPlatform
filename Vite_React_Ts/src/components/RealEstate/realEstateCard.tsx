import { useEffect, useState } from "react";
import realEstate from "../../interface/RealEstate/realEstate";
interface RealEstateProps {
  realEstate: realEstate;
}

const RealEstateCard = ({ realEstate }: RealEstateProps) => {
  const [estate, setEstate] = useState<realEstate | undefined>(realEstate);
  const [formattedDateEnd, setFormattedDateEnd] = useState<string>("");

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
    <div className="max-w-sm bg-white border border-gray-200 rounded-lg shadow mx-auto sm:my-2 md:my-0">
      <div className="">
        <img
          className="rounded-t-lg h-52 w-full"
          src={estate?.uriPhotoFirst}
          alt=""
        />
      </div>
      <div className="p-5">
        <div>
          <h5 className="mb-2 text-xl font-bold tracking-tight text-gray-900 xl:line-clamp-2 md:line-clamp-3 ">
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
            <span className="pl-1">VND</span>
          </div>
        </div>
        <div className="flex justify-end text-gray-700">
          <div className=" tracking-tight">
            Due:{" "}
            <span className="text-gray-900 font-semibold">
              {formattedDateEnd}
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RealEstateCard;
