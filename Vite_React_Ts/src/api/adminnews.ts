import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getNewsAdmin = async (token : string) => {
  try {
    const fetchData = await axios.get<news[]>(`${baseUrl}/api/admin/news`, 
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
export const searchNewsAdmin = async ({
    KeyWord,
}: searchNewsAdmin , token : string) => {
  try {
    const param = {
        KeyWord,
    };
    const fetchData = await axios.get<news[]>(
      `${baseUrl}/api/admin/news/search?KeyWord=${param.KeyWord}`,
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
export const getNewsAdminById = async (id: Number | undefined, token : string) => {
  try {
    const fetchData = await axios.get<newsDetail>(
      `${baseUrl}/api/admin/news/detail/${id}`,
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

export const addNews = async ({
    newsContent,
    newsSumary,
    newsTitle,
    thumbnailUri
}:newscreate, token: string) => {
    try {
        const param ={
            newsContent, newsSumary, newsTitle, thumbnailUri
        }
      const fetchData = await axios.post<Message>(
        `${baseUrl}/api/admin/news/add`,
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

  export const updateNews = async ({
    newsContent,
    newsSumary,
    newsTitle,
    thumbnail,
    dateCreated,
    newsId
}:newsUpdate, token: string) => {
    try {
        const param ={
            newsContent, newsSumary, newsTitle, thumbnail, dateCreated, newsId
        }
      const fetchData = await axios.post<Message>(
        `${baseUrl}/api/admin/news/update`,
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