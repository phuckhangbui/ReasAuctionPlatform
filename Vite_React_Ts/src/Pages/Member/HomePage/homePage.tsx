import RealEstateList from "../../../components/RealEstate/realEstateList";
import NewsList from "../../../components/News/newsList.tsx";
import AuctionList from "../../../components/Auction/auctionList.tsx";
import Banner from "../../../components/Banner/banner.tsx";
import { useEffect, useState } from "react";
import { getRealEstateHome } from "../../../api/realEstate.ts";
import { getNewsHome } from "../../../api/news.ts";
import realEstate from "../../../interface/RealEstate/realEstate.ts";
import { getAuctionHome } from "../../../api/auctions.ts";

const HomePage = () => {
  const [realEstateList, setRealEstateList] = useState<
    realEstate[] | undefined
  >([]);
  const [smallRealEstateList, setSmallRealEstateList] = useState<
    realEstate[] | undefined
  >([]);
  const [newsList, setNewsList] = useState<news[] | undefined>([]);
  const [smallNewsList, setSmallNewsList] = useState<news[] | undefined>([]);
  const [auctionsList, setAuctionsList] = useState<auction[] | undefined>([]);
  const [smallAuctionsList, setSmallAuctionsList] = useState<
    auction[] | undefined
  >([]);

  useEffect(() => {
    try {
      const fetchRealEstates = async () => {
        const response = await getRealEstateHome();
        setRealEstateList(response);
      };
      const fetchNews = async () => {
        const response = await getNewsHome();
        setNewsList(response);
      };
      const fetchAuctions = async () => {
        const response = await getAuctionHome({
          pageNumber: 1,
          pageSize: 100,
          keyword: "",
          timeStart: "",
          timeEnd: "",
        });
        setAuctionsList(response);
      };

      fetchRealEstates();
      fetchNews();
      fetchAuctions();
    } catch (error) {
      console.log(error);
    }
  }, []);

  useEffect(() => {
    if (realEstateList && realEstateList.length > 8) {
      setSmallRealEstateList(realEstateList.slice(0, 8));
    } else {
      setSmallRealEstateList(realEstateList);
    }
  }, [realEstateList]);

  useEffect(() => {
    if (auctionsList && auctionsList.length > 4) {
      setSmallAuctionsList(auctionsList.slice(0, 4));
    } else {
      setSmallAuctionsList(auctionsList);
    }
  }, [auctionsList]);

  useEffect(() => {
    if (newsList && newsList.length > 4) {
      setSmallNewsList(newsList.slice(0, 4));
    } else {
      setSmallNewsList(newsList);
    }
  }, [newsList]);

  return (
    <div>
      <div className="pt-20">
        <Banner />
      </div>
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              What's New Today
            </div>
            <div className="mt-2">
              See the latest news about the current real estates market
            </div>
          </div>
          <NewsList newsList={smallNewsList} />
        </div>
      </div>
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <hr className="mt-6 border-gray-200 sm:mx-auto lg:my-8" />
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              Take Part in Our Most Popular Auctions
            </div>
            <div className="mt-2">
              Participate and try your best to win your dream home
            </div>
          </div>
          <AuctionList auctionsList={smallAuctionsList} />
        </div>
      </div>
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <hr className="mt-6 border-gray-200 sm:mx-auto lg:my-8" />
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              Explore Our Real Estate Options
            </div>
            <div className="mt-2">
              Take a look at our various options and find your forever home
            </div>
          </div>
          <RealEstateList
            realEstatesList={smallRealEstateList}
            ownRealEstates={false}
          />
        </div>
      </div>
    </div>
  );
};

export default HomePage;
