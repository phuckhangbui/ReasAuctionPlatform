import { GoogleLogin } from "@react-oauth/google";
import { googleLogIn } from "../../api/login";
import { useContext } from "react";
import { UserContext } from "../../context/userContext";
import { generateToken } from "../../Config/firebase-config";

interface GoogleLogInProps {
  closeModal: () => void;
  // onSuccess: () => void
}

const GoogleLogIn = ({ closeModal }: GoogleLogInProps) => {
  const { login } = useContext(UserContext);

  const handleOnSuccess = async (credentialResponse: any) => {
    try {
      // console.log(credentialResponse);
      const firebaseToken = await generateToken();
      const idTokenString = String(credentialResponse.credential);
      if (firebaseToken || firebaseToken === "") {
        const response = await googleLogIn(idTokenString, firebaseToken);
        const responseData = response?.data;
        const user = {
          id: responseData?.id,
          accountName: responseData.accountName,
          email: responseData.email,
          roleId: responseData.roleId,
        } as loginUser;
        login(user, responseData.token);
        closeModal();
      }
    } catch (error) {
      console.log(error);
    }
  };
  return (
    <div>
      <div>
        <GoogleLogin
          onSuccess={handleOnSuccess}
          onError={() => {
            console.log("Login Failed");
          }}
        />
      </div>
    </div>
  );
};

export default GoogleLogIn;
