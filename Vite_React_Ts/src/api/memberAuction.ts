import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getAuctionHome = async (reasId: number) => {
  try {
    const fetchData = await axios.get<memberAuction>(
      `${baseUrl}/auctions/${reasId}`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const getAuctionUserList = async (reasId: number) => {
  try {
    const fetchData = await axios.get<number[]>(
      `${baseUrl}/auctions/${reasId}/attenders`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
