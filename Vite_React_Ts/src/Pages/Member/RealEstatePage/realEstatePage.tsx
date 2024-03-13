import { useEffect, useState } from "react";
import SearchBar from "../../../components/SearchBar/searchBar.tsx";
import RealEstateList from "../../../components/RealEstate/realEstateList.tsx";
import { searchRealEstate } from "../../../api/realEstate.ts";
import realEstate from "../../../interface/RealEstate/realEstate.ts";

const priceList = [
  5000, 6000, 7000, 8000, 10000, 11000, 12000, 15000, 20000, 50000, 100000,
  200000, 500000, 600000, 1000000,
];

const RealEstatePage = () => {
  const [dropdownVisible, setDropdownVisible] = useState(false);
  const [minPrice, setMinPrice] = useState<number | null>(0);
  const [maxPrice, setMaxPrice] = useState<number | null>(0);
  const [minPriceList, setMinPriceList] = useState(priceList);
  const [maxPriceList, setMaxPriceList] = useState(priceList);
  const [realEstateList, setRealEstateList] = useState<
    realEstate[] | undefined
  >([]);
  const [searchParams, setSearchParams] = useState<searchRealEstate | null>({
    pageNumber: 1,
    pageSize: 10,
    reasName: "",
    reasPriceFrom: 0,
    reasPriceTo: 0,
    reasStatus: -1,
  });

  useEffect(() => {
    try {
      const fetchRealEstates = async () => {
        if (searchParams) {
          const response = await searchRealEstate(searchParams);
          setRealEstateList(response);
        }
      };
      fetchRealEstates();
    } catch (error) {
      console.log(error);
    }
  }, []);

  // Function to toggle dropdown visibility
  const toggleDropdown = () => {
    setDropdownVisible((preDropdownVisible) => !preDropdownVisible);
  };

  const handleSearchBarChange = async (value: string) => {
    setSearchParams((prevState) => ({
      ...prevState!,
      reasName: value,
    }));
  };

  const handleChangeMinPriceList = async (
    e: React.ChangeEvent<HTMLSelectElement>
  ) => {
    const selectedPrice = parseInt(e.target.value);
    setMinPrice(selectedPrice);
    const currentMaxPriceList = priceList.filter(
      (price) => price > selectedPrice
    );
    setMaxPriceList(currentMaxPriceList);

    setSearchParams((prevState: searchRealEstate | null) => ({
      ...prevState!,
      reasPriceFrom: selectedPrice,
    }));
  };

  const handleChangeMaxPriceList = async (
    e: React.ChangeEvent<HTMLSelectElement>
  ) => {
    const selectedPrice = parseInt(e.target.value);
    setMaxPrice(selectedPrice);
    const currentMinPriceList = priceList.filter(
      (price) => price < selectedPrice
    );
    setMinPriceList(currentMinPriceList);

    setSearchParams((prevState: searchRealEstate | null) => ({
      ...prevState!,
      reasPriceTo: selectedPrice,
    }));
  };

  const handleChangeType = async (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedStatus = parseInt(e.target.value);
    setSearchParams((prevState: searchRealEstate | null) => ({
      ...prevState!,
      reasStatus: selectedStatus,
    }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      const fetchRealEstates = async () => {
        if (searchParams) {
          const response = await searchRealEstate(searchParams);
          setRealEstateList(response);
        }
      };
      fetchRealEstates();
    } catch (error) {
      console.log(error);
    }
    // console.log(searchParams);
  };

  return (
    <>
      <div className="pt-20">
        <form action="" onSubmit={handleSubmit}>
          <div className="w-full relative">
            <img
              src="../../public/Search-bar-bg.jpg"
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
                  placeHolder="Search for real estates that you want"
                  inputName="reasName"
                  nameValue={searchParams?.reasName || ""}
                  onSearchChange={handleSearchBarChange}
                />
                <div className="flex px-3 py-1">
                  <div className="mt-1 flex items-center justify-end">
                    <label htmlFor="status" className="sr-only">
                      Underline select
                    </label>
                    <select
                      id="status"
                      className=" text-gray-900 block py-1 pl-3 pr-5 w-36 text-sm border-2 border-gray-200 appearance-none focus:outline-none focus:ring-0 focus:border-gray-200 rounded-l-lg"
                      onChange={handleChangeType}
                    >
                      <option value="-1">All</option>
                      <option value="2">Available</option>
                      <option value="5">Sold</option>
                      <option value="4">Auctioning</option>
                    </select>
                    <svg
                      className="w-3 h-3 text-gray-900 absolute mr-3"
                      aria-hidden="true"
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 14 8"
                    >
                      <path
                        stroke="currentColor"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth="2"
                        d="m1 1 5.326 5.7a.909.909 0 0 0 1.348 0L13 1"
                      />
                    </svg>
                  </div>
                  <div className="mt-1 w-full flex items-center ">
                    <button
                      id="dropdownBottomButton"
                      className="text-gray-900 bg-white py-1 pl-3 w-full font-medium rounded-r-lg text-sm  text-center inline-flex items-center border-r-2 border-y-2 justify-between pr-1"
                      type="button"
                      onClick={toggleDropdown}
                    >
                      <div>
                        Price:{" "}
                        <span className="font-bold">
                          ${minPrice}{" "}
                          {maxPrice !== null &&
                          maxPrice !== 1000000000 &&
                          maxPrice !== 0
                            ? `- $${maxPrice}`
                            : ""}
                        </span>
                      </div>
                      <svg
                        className="w-3 h-3 text-gray-900 mr-2"
                        aria-hidden="true"
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 14 8"
                      >
                        <path
                          stroke="currentColor"
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="m1 1 5.326 5.7a.909.909 0 0 0 1.348 0L13 1"
                        />
                      </svg>
                    </button>
                    {dropdownVisible && (
                      <div
                        id="dropdownBottom"
                        className="z-5 py-2 px-3 bg-white divide-y divide-gray-100 rounded-b-lg shadow lg:w-86  sm:w-69 absolute mt-30 "
                        aria-labelledby="dropdownBottomButton"
                      >
                        <div className="relative bg-white w-full rounded-lg flex justify-between">
                          <div>
                            <div>Minimum</div>
                            <div className="mt-1 flex items-center justify-end">
                              <label htmlFor="minimum" className="sr-only">
                                Minimum
                              </label>
                              <select
                                id="minimum"
                                className="text-gray-900 block py-1 pl-3 pr-5 lg:w-36 sm:w-28 text-sm border-2 border-gray-400 bg-gray-200 focus:outline-none appearance-none focus:ring-0 focus:border-gray-400 rounded-lg"
                                onChange={handleChangeMinPriceList}
                                name="reasPriceFrom"
                              >
                                <option value="0">$0</option>
                                {minPriceList.map((minPrice) => (
                                  <option value={minPrice} key={minPrice}>
                                    ${minPrice}
                                  </option>
                                ))}
                              </select>
                              <svg
                                className="w-3 h-3 text-gray-900 absolute mr-3"
                                aria-hidden="true"
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 14 8"
                              >
                                <path
                                  stroke="currentColor"
                                  strokeLinecap="round"
                                  strokeLinejoin="round"
                                  strokeWidth="2"
                                  d="m1 1 5.326 5.7a.909.909 0 0 0 1.348 0L13 1"
                                />
                              </svg>
                            </div>
                          </div>

                          <div className="pt-7">
                            <div className="text-2xl"> - </div>
                          </div>
                          <div>
                            <div>Maximum</div>
                            <div className="mt-1 flex items-center justify-end">
                              <label htmlFor="status" className="sr-only">
                                Underline select
                              </label>
                              <select
                                id="status"
                                className=" text-gray-900 block py-1 pl-3 pr-5 lg:w-36 sm:w-28 text-sm border-2 border-gray-400 bg-gray-200 focus:outline-none appearance-none focus:ring-0 focus:border-gray-400 rounded-lg"
                                onChange={handleChangeMaxPriceList}
                                name="reasPriceTo"
                              >
                                <option value="1000000000">Any Price</option>
                                {maxPriceList.map((maxPrice) => (
                                  <option value={maxPrice} key={maxPrice}>
                                    ${maxPrice}
                                  </option>
                                ))}
                              </select>
                              <svg
                                className="w-3 h-3 text-gray-900 absolute mr-3"
                                aria-hidden="true"
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 14 8"
                              >
                                <path
                                  stroke="currentColor"
                                  strokeLinecap="round"
                                  strokeLinejoin="round"
                                  strokeWidth="2"
                                  d="m1 1 5.326 5.7a.909.909 0 0 0 1.348 0L13 1"
                                />
                              </svg>
                            </div>
                          </div>
                        </div>
                      </div>
                    )}
                  </div>
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
              Explore Our Real Estate Options
            </div>
            <div className="mt-2">
              Take a look at our various options and find your forever home
            </div>
          </div>
          <RealEstateList realEstatesList={realEstateList} />
        </div>
      </div>
    </>
  );
};

export default RealEstatePage;
