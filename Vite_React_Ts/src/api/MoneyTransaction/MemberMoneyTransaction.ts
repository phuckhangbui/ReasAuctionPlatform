import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getTransactionHistory = async (userId: number, token: string) => {
  try {
    const fetchData = await axios.get<transactionHistory[]>(
      `${baseUrl}/api/transactions/member/${userId}`,
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

export const getTransactionDetail = async (
  userId: number,
  transactionId: number,
  token: string
) => {
  try {
    const fetchData = await axios.get<TransactionDetail>(
      `${baseUrl}/api/transactions/member/${userId}/${transactionId}`,
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
