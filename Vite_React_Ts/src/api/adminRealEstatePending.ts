import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getRealEstatePending = async (token: string) => {
  try {
    const fetchData = await axios.get<ManageRealEstate[]>(
      `${baseUrl}/api/admin/real-estate/pending`,
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
export const searchManageRealEstate = async ({
  reasName,
  reasPriceFrom,
  reasPriceTo,
  reasStatus,
}: searchManageRealEstate, token: string) => {
  try {
    const param = {
      reasName,
      reasPriceFrom,
      reasPriceTo,
      reasStatus,
    };
    const fetchData = await axios.get<ManageRealEstate[]>(
      `${baseUrl}/api/admin/real-estate/pending/search?reasName=${param.reasName}&reasPriceFrom=${param.reasPriceFrom}&reasPriceTo=${param.reasPriceTo}&reasStatus=${param.reasStatus}`,
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

export const getRealEstateById = async (id: Number | undefined, token : string) => {
  try {
    const fetchData = await axios.get<ManageRealEstateDetail>(
      `${baseUrl}/api/admin/real-estate/pending/detail/${id}`,
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

export const changeStatusRealPending = async (reasId: Number | undefined, reasStatus: Number | undefined, messageString: string | undefined, token: string) => {
    try {
        const param ={
          reasId, reasStatus, messageString
        }
      const fetchData = await axios.post<Message>(
        `${baseUrl}/api/admin/real-estate/change/`,
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
      console.log("Error: " + error);
    }
  };
