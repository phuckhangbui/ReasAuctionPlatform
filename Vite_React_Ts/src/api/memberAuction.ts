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
    return response;
  } catch (error) {
    console.log("Error:", error);
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

export const StartAuction = async (auctionId: number, token: string) => {
  try {
    const fetchData = await axios.get<Message>(
      `${baseUrl}/api/Auction/start?auctionId=${auctionId}`,
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
    console.log("Error: " + error);
  }
};

export const auctionSuccess = async (
  { auctionId, accountWinId, winAmount }: auctionFinish,
  userList: userHistory[],
  token: string
) => {
  try {
    const param = {
      auctionDetailDto: {
        auctionId,
        accountWinId,
        winAmount,
      },
      auctionHistory: userList,
    };
    const fetchData = await axios.post<Message>(
      `${baseUrl}/api/Auction/success`,
      param,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    console.log("test");
    console.log(param);
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.error("Error submitting auction success data:", error);
    throw error;
  }
};

export const GetAuctionHistory = async (accountId: number, token: string) => {
  try {
    const fetchData = await axios.get<auctionHistory[]>(
      `${baseUrl}/api/Auction/auctions/attend/history?AccountId=${accountId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
