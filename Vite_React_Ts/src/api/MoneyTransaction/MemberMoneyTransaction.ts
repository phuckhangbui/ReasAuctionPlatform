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
    console.log(response);
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
