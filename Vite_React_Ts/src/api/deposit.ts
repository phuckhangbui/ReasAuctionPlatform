import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getDeposit = async (token: string) => {
  try {
    const fetchData = await axios.get<deposit[]>(
      `${baseUrl}/api/deposits/`
      ,
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

export const getReasDeposited = async (token: string, reasId: number) => {
  try {
    const fetchData = await axios.get<DepositAmountUser[]>(
      `${baseUrl}/api/deposits/${reasId}`,
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

export const getReasName = async (token: string, reasId: number) => {
  try {
    const fetchData = await axios.get<string>(
      `${baseUrl}/api/admin/real-estate/name/${reasId}`,
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
