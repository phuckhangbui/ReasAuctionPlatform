import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getTransaction = async (
  token: string,
  dateExecutionFrom: string | undefined,
  dateExecutionTo: string | undefined
) => {
  try {
    const fetchData = await axios.post<transaction[]>(
      `${baseUrl}/api/transactions/`,
      {
        dateExecutionFrom,
        dateExecutionTo,
      },
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
    console.log("Error: ", error);
  }
};

export const getTransactionDetail = async (token: string, id: number) => {
  try {
    const fetchData = await axios.get<transactionDetail>(
      `${baseUrl}/api/transactions/${id}`,
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
    console.log("Error: ", error);
  }
};

// Auction Transaction
export const registerParticipateAuction = async (
  userId: number,
  reasId: number,
  token: string
) => {
  try {
    const fetchData = await axios.post<depositRegister>(
      `${baseUrl}/api/Auction/register`,
      {
        accountId: userId,
        reasId: reasId,
        returnUrl: "http://localhost:5173/success",
      },
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

export const payDeposit = async (
  queryString: string,
  depositId: number,
  token: string
) => {
  try {
    const fetchData = await axios.post(
      `${baseUrl}/api/Auction/pay/deposit/returnUrl/${depositId}`,
      {
        url: queryString,
      },
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

// Real Estate Transaction
export const payRealEstatePostingFee = async (
  userId: number,
  reasId: number,
  token: string
) => {
  try {
    const fetchData = await axios.post(
      `${baseUrl}/api/home/my_real_estate/detail/${reasId}/createPaymentLink?`,
      {
        accountId: userId,
        reasId: reasId,
        returnUrl: "http://localhost:5173/success",
      },
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

export const payRealEstate = async (
  queryString: string,
  reasId: number,
  token: string
) => {
  try {
    const fetchData = await axios.post(
      `${baseUrl}/api/home/pay/fee/returnUrl/${reasId}`,
      {
        url: queryString,
      },
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    const response = fetchData.data;
    console.log(response)
    return response;
  } catch (error) {
    console.log("Error:", error);
  }
};
