import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getAuctionHome = async () => {
  try {
    const fetchData = await axios.get<auction[]>(
      `${baseUrl}/api/Auction/auctions`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
