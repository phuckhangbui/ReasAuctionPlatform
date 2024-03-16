import realEstate from "../../interface/RealEstate/realEstate";

interface RealEstateProps {
  realEstate: realEstate;
}

const AuctionCard = ({ realEstate }: RealEstateProps) => {
  return (
    <div className="max-w-sm bg-white border border-gray-200 rounded-lg shadow mx-auto sm:my-2 md:my-0">
      <div>
        {/* <img className="rounded-t-lg" src={realEstate.image} alt="" /> */}
      </div>
      <div className="p-5">
        <div>
          <h5 className="mb-2 text-2xl font-bold tracking-tight text-gray-900 truncate ">
            {realEstate.reasName}
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
