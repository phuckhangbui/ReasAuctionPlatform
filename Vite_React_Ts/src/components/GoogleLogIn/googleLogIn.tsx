import { GoogleLogin } from "@react-oauth/google";
import { googleLogIn } from "../../api/login";
import { useContext } from "react";
import { UserContext } from "../../context/userContext";
import { generateToken } from "../../Config/firebase-config";
import toast from "react-hot-toast";

interface GoogleLogInProps {
  closeModal: () => void;
  // onSuccess: () => void
}

const GoogleLogIn = ({ closeModal }: GoogleLogInProps) => {
  const { login } = useContext(UserContext);

  const handleOnSuccess = async (credentialResponse: any) => {
    try {
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
          isNewAccount: responseData.isNewAccount
        } as loginUser;
        login(user, responseData.token);
        if (user.isNewAccount === true) {
          toast.custom((t) => (
            <div
              className={`${
                t.visible ? "animate-enter" : "animate-leave"
              } max-w-sm w-full bg-white shadow-lg rounded-lg pointer-events-auto flex ring-1 ring-black ring-opacity-5`}
            >
              <div className="flex-1 w-0 p-1.5">
                <div className="flex items-center justify-center">
                  <div className="flex-shrink-0 pl-1">
                    <img
                      src="./REAS-removebg-preview.png"
                      className="h-8"
                      alt="Flowbite Logo"
                    />
                  </div>
                  <div className="ml-3 flex-1">
                    <p className="text-lg font-medium text-gray-900">
                      Welcome, {user.accountName}
                    </p>
                    <p className=" text-sm text-gray-500">
                      Thank you for choosing us, together we will help you find
                      great fortune
                    </p>
                  </div>
                </div>
              </div>
            </div>
          ));
        }
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
