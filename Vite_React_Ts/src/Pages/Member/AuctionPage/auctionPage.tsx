import React, { useEffect, useState } from "react";
import SearchBar from "../../../components/SearchBar/searchBar";
import { DatePicker } from "antd";
import dayjs from "dayjs";
import AuctionList from "../../../components/Auction/auctionList";
import { getAuctionHome } from "../../../api/auctions";

const AuctionPage = () => {
  const [auctionsList, setAuctionsList] = useState<auction[] | undefined>([]);
  const [searchParams, setSearchParams] = useState<searchAuction | null>({
    pageNumber: 1,
    pageSize: 100,
    keyword: "",
    timeStart: "",
    timeEnd: "",
  });
  const { RangePicker } = DatePicker;

  useEffect(() => {
    try {
      const fetchAuctions = async () => {
        if (searchParams) {
          const response = await getAuctionHome(searchParams);
          setAuctionsList(response);
        }
      };
      fetchAuctions();
    } catch (error) {
      console.log(error);
    }
  }, []);

  const handleSearchBarChange = async (value: string) => {
    setSearchParams((prevState) => ({
      ...prevState!,
      keyword: value,
    }));
  };

  const handleDateRange = (
    dates: [dayjs.Dayjs | null, dayjs.Dayjs | null],
    dateStrings: [string, string]
  ) => {
    if (dates && dates.length === 2) {
      setSearchParams((prevState) => ({
        ...prevState!,
        timeStart: dateStrings[0],
      }));
      setSearchParams((prevState) => ({
        ...prevState!,
        timeEnd: dateStrings[1],
      }));
    }
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      const fetchAuctions = async () => {
        console.log(searchParams);
        if (searchParams) {
          const response = await getAuctionHome(searchParams);
          setAuctionsList(response);
        }
      };
      fetchAuctions();
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="">
      <div className="pt-20">
        <form action="post" onSubmit={handleSubmit}>
          <div className="w-full relative">
            <img
              src="./Search-bar-bg.jpg"
              alt=""
              className="w-full md:h-96 sm:h-72 object-cover"
            />
            <div className="absolute inset-0 flex items-center justify-center">
              <div className="lg:max-w-lg sm:max-w-md mx-auto w-full">
                <div className="text-center lg:text-4xl sm:text-3xl lg:mb-4 sm:mb-2 text-white font-bold">
                  Find. <span className="text-mainBlue">Auction.</span> Deposit.{" "}
                  <span className="text-secondaryYellow">Own.</span>
                </div>
                <SearchBar
                  placeHolder="Search for the auction you want to see"
                  inputName="keyword"
                  nameValue={searchParams?.keyword || ""}
                  onSearchChange={handleSearchBarChange}
                />
                <div className="px-3 py-2 w-full">
                  <RangePicker onChange={handleDateRange} className="w-full" />
                </div>
                <div className="w-full">
                  <button type="submit" className="w-full text-transparent">
                    Submit
                  </button>
                </div>
              </div>
            </div>
          </div>
        </form>
      </div>
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              Take Part in Our Most Popular Auctions
            </div>
            <div className="mt-2">
              Participate and try your best to win your dream home
            </div>
          </div>
          <AuctionList auctionsList={auctionsList} />
        </div>
      </div>
    </div>
  );
};

export default AuctionPage;
