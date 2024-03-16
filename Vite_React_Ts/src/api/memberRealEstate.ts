import axios from "axios";
import realEstate from "../interface/RealEstate/realEstate";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getMemberRealEstates = async (token: string) => {
  try {
    const fetchData = await axios.get<realEstate[]>(
      `${baseUrl}/api/home/my_real_estate`,
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


