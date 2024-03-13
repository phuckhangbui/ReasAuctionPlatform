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

export const getAuctionStatus = async (
  userId: number,
  reasId: number,
  token: string
) => {
  try {
    const fetchData = await axios.get<auctionStatus>(
      `${baseUrl}/api/home/customer/auction/status?customerId=${userId}&reasId=${reasId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    const response = fetchData.data;
    console.log(response);
    return response;
  } catch (error) {
    console.log("Error:", error);
  }
};

export const registerParticipateAuction = async (
  userId: number,
  reasId: number,
  token: string
) => {
  try {
    const fetchData = await axios.get<depositRegister>(
      `${baseUrl}/api/Auction/register?customerId=${userId}&reasId=${reasId}&returnUrl=http://localhost:5173/success`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    const response = fetchData.data;
    console.log(response);
    return response;
  } catch (error) {
    console.log("Error:", error);
  }
};
