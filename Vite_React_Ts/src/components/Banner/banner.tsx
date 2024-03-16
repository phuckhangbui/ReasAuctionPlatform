import React from "react";

const Banner = () => {
  return (
    <div className="w-full relative">
      <img
        src="../../public/Banner.jpg"
        alt=""
        className="w-full md:h-96 sm:h-72 object-cover"
      />
      <div className="container w-full mx-auto absolute inset-0 flex lg:items-start sm:items-center justify-center flex-col text-center md:text-6xl sm:text-5xl text-white font-bold">
        <div className="">
          Find. <span className="text-mainBlue">Auction.</span>
        </div>
        <div className="lg:ml-32 ">
          Deposit. <span className="text-secondaryYellow">Own.</span>
        </div>
      </div>
    </div>
  );
};

export default Banner;
