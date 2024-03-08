import { ReactNode, createContext, useEffect, useState } from "react";

interface UserProviderProps {
  children: ReactNode;
}

interface UserContextType {
  userRole: number | undefined;
  token: string | undefined;
  login: (user: loginUser, token: string, userId: number) => void;
  logout: () => void;
  isAuth: () => boolean;
}

export const UserContext = createContext<UserContextType>({
  userRole: undefined,
  token: undefined,
  login: () => {},
  logout: () => {},
  isAuth: () => false,
});

const UserProvider = ({ children }: UserProviderProps) => {
  const [userRole, setUserRole] = useState<number | undefined>(undefined);
  const [token, setToken] = useState<string | undefined>(undefined);

  const login = (user: loginUser, token: string, userId: number) => {
    const stringUser = JSON.stringify(user);
    localStorage.setItem("userLogin", stringUser);
    localStorage.setItem("token", token);
    localStorage.setItem("userId", userId.toString());
    setUserRole(user?.roleId);
    setToken(token);
  };

  useEffect(() => {
    try {
      const getLocalData = async () => {
        const storageToken = localStorage.getItem("token");
        const storageUser = localStorage.getItem("user");
        const parseStorageUser = JSON.parse(storageUser as string);
        if (storageToken && storageUser) {
          setUserRole(parseStorageUser?.roleId);
          setToken(storageToken);
        }
      };
      getLocalData();
    } catch (error) {
      console.log(error);
    }
  }, []);

  // useEffect(() => {
  //   console.log("User Role: ", userRole);
  // }, [userRole]);

  const logout = () => {
    setUserRole(undefined);
    setToken(undefined);
    localStorage.removeItem("user");
    localStorage.removeItem("token");
  };

  const isAuth = () => {
    const storageToken = localStorage.getItem("token");
    if (storageToken && storageToken !== undefined) {
      return true;
    } else {
      return false;
    }
  };

  return (
    <UserContext.Provider value={{ userRole, token, login, logout, isAuth }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;
