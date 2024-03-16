import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getRealEstateAll = async (token: string) => {
  try {
    const fetchData = await axios.get<ManageRealEstate[]>(
      `${baseUrl}/api/admin/real-estate/all`,
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
    const statusParams = reasStatus.map(status => `reasStatus=${status}`).join('&');
    const fetchData = await axios.get<ManageRealEstate[]>(
      `${baseUrl}/api/admin/real-estate/all/search?reasName=${param.reasName}&reasPriceFrom=${param.reasPriceFrom}&reasPriceTo=${param.reasPriceTo}&${statusParams}`,
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
      `${baseUrl}/api/admin/real-estate/all/detail/${id}`,
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

export const changeStatusRealAll = async (reasId: Number | undefined, reasStatus: Number | undefined, messageString: string | undefined, token: string) => {
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
