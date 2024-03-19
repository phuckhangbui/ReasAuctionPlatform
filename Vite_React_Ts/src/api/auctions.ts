import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getAuctionHome = async ({
  keyword,
  timeStart,
  timeEnd,
  pageNumber,
  pageSize,
}: searchAuction) => {
  try {
    const searchBody = {
      keyword,
      timeStart,
      timeEnd,
      pageNumber,
      pageSize,
    };
    const fetchData = await axios.post<auction[]>(
      `${baseUrl}/api/Auction/auctions`,
      searchBody
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
