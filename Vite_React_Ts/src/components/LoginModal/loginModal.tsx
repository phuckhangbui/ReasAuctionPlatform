import { useContext, useState } from "react";
import GoogleLogIn from "../GoogleLogIn/googleLogIn";
import { SubmitHandler, useForm } from "react-hook-form";
import { staffLogin } from "../../api/login";
import { notification } from "antd";
import { UserContext } from "../../context/userContext";
import { useNavigate } from "react-router-dom";

interface LoginModalProps {
  closeModal: () => void;
}

const LoginModal = ({ closeModal }: LoginModalProps) => {
  const [tabStatus, setTabStatus] = useState("user");
  const [showPassword, setShowPassword] = useState(false);
  const { register, handleSubmit } = useForm<loginStaff>();
  const { login } = useContext(UserContext);
  const navigate = useNavigate();

  // Change the tab index
  const toggleTab = (type: string) => {
    setTabStatus(type);
  };

  // Tab button status
  const getActiveTab = (type: string) => {
    return `${
      type === tabStatus
        ? "text-mainBlue border-mainBlue font-bold h-0"
        : "border-transparent hover:border-gray-900 max-w-full group-hover:max-w-full transition-all duration-500 bg-sky-600 h-0"
    } text-xl border-b-2 rounded-t-lg `;
  };

  // Changing the tab description
  const getActiveTabDetail = (type: string) => {
    return `${type === tabStatus ? "" : "hidden"} mt-2 space-y-4 `;
  };

  const handleSeePassword = (e: React.FormEvent<HTMLButtonElement>) => {
    e.preventDefault();
    setShowPassword((prevStatus) => !prevStatus);
  };

  // const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
  //   e.preventDefault();
  //   try {
  //     const loginStaff = async () => {};
  //     loginStaff();
  //   } catch (error) {
  //     console.log(error);
  //   }
  // };
  const onSubmit: SubmitHandler<loginStaff> = (data) => {
    try {
      let check : boolean = true;
      const loginStaff = async () => {
        const response = await staffLogin(data);
        const responseData = response?.data;
        if(responseData?.id == null){
          check = false;
          openNotificationWithIcon("error", "Password or Username is not correct. Try again!");
        }else{
          const user = {
            id: responseData?.id,
            accountName: responseData?.accountName,
            email: responseData?.email,
            roleId: responseData?.roleId,
            username: responseData?.username,
          } as loginUser;
          if (responseData?.token) {
            login(user, responseData?.token);
          }
          navigate("/admin");
        }
      };
        loginStaff();
    } catch (error) {
      console.log(error);
    }
  };

  const openNotificationWithIcon = (
    type: "success" | "error",
    description: string
  ) => {
    notification[type]({
      message: "Notification Title",
      description: description,
    });
  };

  return (
    <div className="relative p-4 w-full max-w-md max-h-full">
      <div className="relative bg-white rounded-lg shadow ">
        <div className="p-4 md:p-5 border-b rounded-t text-center ">
          <h3 className="text-2xl  font-bold text-gray-900 ">
            {tabStatus !== "forgot" ? "Sign in" : "Forgot Password"}
          </h3>
        </div>
        <div className=" grid grid-cols-2 transition duration-300 group mb-8">
          <button
            className={getActiveTab("user")}
            onClick={() => toggleTab("user")}
          >
            <div className="mt-2">User</div>
          </button>
          <button
            className={getActiveTab("staff")}
            onClick={() => toggleTab("staff")}
          >
            <div className="mt-2">Staff</div>
          </button>
        </div>
        <div className={getActiveTabDetail("staff")}>
          <div className="p-4 md:p-5">
            <form
              className="space-y-4"
              action="#"
              onSubmit={handleSubmit(onSubmit)}
            >
              <div>
                <label
                  htmlFor="username"
                  className="block mb-2 text-sm font-medium text-gray-900 "
                >
                  Username:
                </label>
                <div>
                  <input
                    type="text"
                    {...register("username")}
                    id="username"
                    className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:outline-none focus:ring-mainBlue focus:border-mainBlue block w-full p-2.5 "
                    placeholder="Type your username"
                  />
                </div>
              </div>

              <label
                htmlFor="password"
                className="block mb-2 text-sm font-medium text-gray-900"
                style={{ marginBottom: "0.5rem" }}
              >
                Password:
              </label>
              <div
                className="flex justify-end items-center mt-0"
                style={{ marginTop: "0px" }}
              >
                <input
                  type={showPassword ? "text" : "password"}
                  id="password"
                  placeholder="Type your password"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:outline-none focus:ring-mainBlue focus:border-mainBlue block w-full p-2.5"
                  {...register("password")}
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      handleSubmit(onSubmit)();
                    }
                  }}
                />
                <button className="absolute mr-3" onClick={handleSeePassword}>
                  <svg
                    className="w-6 h-6 text-gray-900 sm:hover:text-mainBlue "
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      strokeWidth="2"
                      d="M21 12c0 1.2-4 6-9 6s-9-4.8-9-6c0-1.2 4-6 9-6s9 4.8 9 6Z"
                    />
                    <path
                      stroke="currentColor"
                      strokeWidth="2"
                      d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                    />
                  </svg>
                </button>
              </div>
              <div className="flex justify-end">
                <div
                  className="text-sm text-mainBlue hover:underline cursor-pointer"
                  onClick={() => toggleTab("forgot")}
                >
                  Forgot Password?
                </div>
              </div>
              <button
                type="submit"
                className="w-full text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-mainBlue font-medium rounded-lg text-sm px-5 py-2.5 text-center "
              >
                Login
              </button>
            </form>
          </div>
        </div>
        <div className={getActiveTabDetail("user")}>
          <div className="flex justify-center items-center p-8">
            <GoogleLogIn closeModal={closeModal} />
          </div>
        </div>
        <div className={getActiveTabDetail("forgot")}>
          <div className="p-4 md:p-5">
            <form action="" className="space-y-4">
              <label
                htmlFor="email"
                className="block mb-2 text-sm font-medium text-gray-900 "
              >
                Enter your email to reset your password
              </label>
              <div>
                <input
                  type="text"
                  name="email"
                  id="email"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-mainBlue focus:border-mainBlue focus:outline-none block w-full p-2.5 "
                  placeholder="Type your email"
                />
              </div>
              <button
                type="submit"
                className="w-full text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-mainBlue font-medium rounded-lg text-sm px-5 py-2.5 text-center"
              >
                Send
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginModal;
