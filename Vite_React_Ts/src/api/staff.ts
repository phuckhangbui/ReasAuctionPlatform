import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getStaff = async (token: string) => {
  try {
    const fetchData = await axios.get<Staff[]>(
      `${baseUrl}/api/admin/user/staff`,
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
export const searchStaff = async ({
  KeyWord,
}: searchStaff, token : string) => {
  try {
    const param = {
        KeyWord,
    };
    const fetchData = await axios.get<Staff[]>(
      `${baseUrl}/api/admin/user/staff/search?KeyWord=${param.KeyWord}`,
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

export const getAccountStaffId = async (id: Number | undefined, token: string) => {
  try {
    const fetchData = await axios.get<staffDetail>(
      `${baseUrl}/api/admin/user/staff/detail/${id}`,
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

export const changeStatusStaff = async (accountId: Number | undefined, accountStatus: Number | undefined, token: string) => {
  try {
      const param ={
          accountId, accountStatus
      }
    const fetchData = await axios.post<Message>(
      `${baseUrl}/api/admin/user/change/`,
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
