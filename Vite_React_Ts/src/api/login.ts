import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const googleLogIn = async (
  idTokenString: string,
  firebaseRegisterToken: string
) => {
  try {
    const response = await axios.post(
      `${baseUrl}/api/Account/login/google`,
      { idTokenString, firebaseRegisterToken },
      {
        headers: {
          Authorization: `Bearer ${idTokenString}`,
          "Content-Type": "application/json",
        },
      }
    );
    console.log(response);
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const staffLogin = async ({ password, username }: loginStaff) => {
  try {
    const param = { password, username };
    const response = await axios.post<loginUser>(
      `${baseUrl}/api/Account/login/admin`,
      param
    );
    return response;
  } catch (error) {
    console.log(error);
  }
};
