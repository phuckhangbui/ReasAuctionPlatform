import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getNewsHome = async () => {
  try {
    const fetchData = await axios.get<news[]>(`${baseUrl}/api/home/news`);
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const searchNews = async ({
  pageNumber,
  pageSize,
  keyWord,
}: searchNews) => {
  try {
    const param = {
      pageNumber,
      pageSize,
      keyWord,
    };
    const fetchData = await axios.post<news[]>(
      `${baseUrl}/api/home/news/search`,
      param
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
export const getNewsById = async (id: number) => {
  try {
    const fetchData = await axios.get<newsDetail>(
      `${baseUrl}/api/home/news/detail/${id}`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const addNews = async ({
  newsContent, newsSumary,newsTitle,thumbnailUri
}:newscreate, token: string) => {
  try {
      const param ={
        newsContent, newsSumary,newsTitle,thumbnailUri
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
