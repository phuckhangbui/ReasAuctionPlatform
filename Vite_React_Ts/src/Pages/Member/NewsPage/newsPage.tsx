import { useEffect, useState } from "react";
import NewsList from "../../../components/News/newsList";
import SearchBar from "../../../components/SearchBar/searchBar";
import { searchNews } from "../../../api/news";

const NewsPage = () => {
  const [newsList, setNewsList] = useState<news[] | undefined>([]);
  const [searchParams, setSearchParams] = useState<searchNews | null>({
    pageNumber: 0,
    pageSize: 0,
    keyWord: "",
  });
  useEffect(() => {
    try {
      const fetchNews = async () => {
        if (searchParams) {
          const response = await searchNews(searchParams);
          setNewsList(response);
        }
      };
      fetchNews();
    } catch (error) {
      console.log(error);
    }
  }, []);

  const handleSearchBarChange = async (value: string) => {
    setSearchParams((prevState) => ({
      ...prevState!,
      keyWord: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      const fetchRealEstates = async () => {
        if (searchParams) {
          const response = await searchNews(searchParams);
          setNewsList(response);
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
                  placeHolder="Search for the news you want to read"
                  inputName="keyWord"
                  nameValue={searchParams?.keyWord || ""}
                  onSearchChange={handleSearchBarChange}
                />
              </div>
            </div>
          </div>
        </form>
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
          {newsList !== null ? (
            <NewsList newsList={newsList} />
          ) : (
            <div>Loading...</div>
          )}
        </div>
      </div>
    </>
  );
};

export default NewsPage;
