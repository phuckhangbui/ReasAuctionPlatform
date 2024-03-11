import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getDeposit = async (token: string, reasName: string) => {
  try {
    const fetchData = await axios.get<deposit[]>(
      `${baseUrl}/api/deposits/`
      ,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
        params: {
          reasName: reasName
        }
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
    const fetchData = await axios.get<depositDetail[]>(
      `${baseUrl}/api/deposits/${reasId}/deposited`,
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
