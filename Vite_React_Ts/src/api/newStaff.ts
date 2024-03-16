import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const addNewStaff = async ({
    accountEmail,
    accountName,
    address,
    citizen_identification,
    passwordHash,
    phoneNumber,
    username,
}:NewStaff, token: string) => {
    try {
        const param ={
            accountEmail,accountName,address,citizen_identification,passwordHash,phoneNumber,username
        }
      const fetchData = await axios.post<Message>(
        `${baseUrl}/api/admin/user/create`,
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