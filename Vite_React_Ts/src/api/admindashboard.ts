import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getTypeReal = async (token : string) => {
  try {
    const fetchData = await axios.get<TypeReas[]>(`${baseUrl}/admin/dashboard/real/type`, 
    {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const getRealMonth = async (token : string) => {
    try {
      const fetchData = await axios.get<ReasMonth[]>(`${baseUrl}/admin/dashboard/real/month`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };

  export const getTotal = async (token : string) => {
    try {
      const fetchData = await axios.get<Total>(`${baseUrl}/admin/dashboard/total`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };

  export const getNews = async (token : string) => {
    try {
      const fetchData = await axios.get<news[]>(`${baseUrl}/admin/dashboard/news`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };

  export const getTotalRenueve = async (token : string) => {
    try {
      const fetchData = await axios.get<number>(`${baseUrl}/admin/dashboard/totalrevenue`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };

  export const getUserListJoinAuction = async (token : string) => {
    try {
      const fetchData = await axios.get<UserJoinAuction[]>(`${baseUrl}/admin/dashboard/listusers`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };

  export const getStaffActive = async (token : string) => {
    try {
      const fetchData = await axios.get<number>(`${baseUrl}/admin/dashboard/staffs`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };