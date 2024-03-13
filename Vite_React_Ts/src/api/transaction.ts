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

export const payDeposit = async (
  params: Record<string, string>,
  depositId: number,
  token: string
) => {
  try {
    const fetchData = await axios.post(
      `${baseUrl}/api/Auction/pay/deposit/returnUrl/${depositId}/`,
      {
        params,
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
