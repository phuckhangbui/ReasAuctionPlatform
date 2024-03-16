import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getRuleHome = async (token: string) => {
  try {
    const fetchData = await axios.get<Rules[]>(`${baseUrl}/api/admin/rule`,
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


export const addRule = async ({
  content,title
}:RuleAdd, token: string) => {
  try {
      const param ={
        content,title
      }
    const fetchData = await axios.post<Message>(
      `${baseUrl}/api/admin/rule/add`,
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

export const ruleUpdate = async ({
    content,idRule
  }:RuleUpdate, token: string) => {
    try {
        const param ={
          content,idRule
        }
      const fetchData = await axios.post<Message>(
        `${baseUrl}/api/admin/rule/update`,
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

  export const getRuleAdminById = async (id: Number | undefined, token : string) => {
    try {
      const fetchData = await axios.get<Rule>(
        `${baseUrl}/api/admin/rule/detail/${id}`,
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