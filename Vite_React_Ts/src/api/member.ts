import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getMember = async (token: string) => {
  try {
    const fetchData = await axios.get<Member[]>(
      `${baseUrl}/api/admin/user/member`,
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
export const searchMember = async (
  { KeyWord }: searchMember,
  token: string
) => {
  try {
    const param = {
      KeyWord,
    };
    const fetchData = await axios.get<Member[]>(
      `${baseUrl}/api/admin/user/member/search?KeyWord=${param.KeyWord}`,
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

export const getAccountMemberId = async (
  id: Number | undefined,
  token: string
) => {
  try {
    const fetchData = await axios.get<memberDetail>(
      `${baseUrl}/api/admin/user/member/detail/${id}`,
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

export const changeStatusMember = async (
  accountId: Number | undefined,
  accountStatus: Number | undefined,
  token: string
) => {
  try {
    const param = {
      accountId,
      accountStatus,
    };
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

export const getMemberProfile = async (token: string) => {
  try {
    const fetchData = await axios.get<memberProfile>(
      `${baseUrl}/api/MemberAccount/profile`,
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

export const updateMemberProfile = async (
  updatedMember: updatedMember,
  token: string
) => {
  try {
    const fetchData = await axios.put(
      `${baseUrl}/api/MemberAccount/profile/update`,
      {
        accountId: updatedMember?.accountId,
        accountName: updatedMember?.accountName,
        address: updatedMember?.address,
        bankingCode: updatedMember?.bankingCode,
        bankingNumber: updatedMember?.bankingNumber,
        citizenIdentification: updatedMember?.citizenIdentification,
        majorId: updatedMember?.majorId,
        phoneNumber: updatedMember?.phoneNumber,
        userName: updatedMember?.username,
      },
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
