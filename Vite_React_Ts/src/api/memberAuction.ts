import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getAuctionHome = async (reasId: number) => {
  try {
    const fetchData = await axios.get<memberAuction>(
      `${baseUrl}/auctions/${reasId}`
    );
    const response = fetchData.data;
    console.log(`${baseUrl}/auctions/${reasId}`);
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
