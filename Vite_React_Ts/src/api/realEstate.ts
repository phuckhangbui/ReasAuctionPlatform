import axios from "axios";
import realEstate from "../interface/RealEstate/realEstate";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getRealEstateHome = async () => {
  try {
    const fetchData = await axios.get<realEstate[]>(
      `${baseUrl}/api/home/real_estate`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const searchRealEstate = async ({
  pageNumber,
  pageSize,
  reasName,
  reasPriceFrom,
  reasPriceTo,
  reasStatus,
}: searchRealEstate) => {
  try {
    const param = {
      pageNumber,
      pageSize,
      reasName,
      reasPriceFrom,
      reasPriceTo,
      reasStatus,
    };
    const fetchData = await axios.post<realEstate[]>(
      `${baseUrl}/api/home/real_estate/search`,
      param
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const getRealEstateById = async (id: number) => {
  try {
    const fetchData = await axios.get<realEstateDetail>(
      `${baseUrl}/api/home/real_estate/detail/${id}`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const getRealEstateType = async (token: string | undefined) => {
  try {
    const fetchData = await axios.get<realEstateType[]>(
      `${baseUrl}/api/home/my_real_estate/view`,
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

export const createRealEstate = async (
  token: string | undefined,
  realEstateInfo: createRealEstate
) => {
  try {
    const fetchData = await axios.post<Message>(
      `${baseUrl}/api/home/my_real_estate/create`,
      realEstateInfo,
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

export const reUpRealEstate = async ({reasId, dateEnd}: ReupRealEstate, token : string | undefined) => {
  try {
    const param = {
      reasId, dateEnd
    }
    const fetchData = await axios.post<Message>(
      `${baseUrl}/api/home/my_real_estate/create`,
      param,
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