import { ReactNode, createContext, useEffect, useState } from "react";

interface UserProviderProps {
  children: ReactNode;
}

interface UserContextType {
  userRole: number | undefined;
  userId: number | undefined;
  token: string | undefined;
  userAccountName: string | undefined;
  login: (user: loginUser, token: string) => void;
  logout: () => void;
  isAuth: () => boolean;
}

export const UserContext = createContext<UserContextType>({
  userRole: undefined,
  userId: undefined,
  token: undefined,
  userAccountName: undefined,
  login: () => {},
  logout: () => {},
  isAuth: () => false,
});

const UserProvider = ({ children }: UserProviderProps) => {
  const [userRole, setUserRole] = useState<number | undefined>();
  const [userAccountName, setUserAccountName] = useState<string | undefined>();
  const [userId, setUserId] = useState<number | undefined>();
  const [token, setToken] = useState<string | undefined>();

  useEffect(() => {
    try {
      const getLocalData = async () => {
        const storageToken = localStorage.getItem("token");
        const storageUser = localStorage.getItem("user");
        const storageExpiration = localStorage.getItem("expiration");
        if (storageExpiration) {
          const expirationDate = parseInt(storageExpiration);
          const currentDate = new Date().getTime();
          if (expirationDate - currentDate <= 0) {
            logout();
          } else {
            if (storageToken && storageUser) {
              const parseStorageUser = JSON.parse(storageUser as string);
              setUserAccountName(parseStorageUser.accountName);
              setUserRole(parseStorageUser.roleId);
              setUserId(parseStorageUser.id);
              setToken(storageToken);
            }
          }
        }
      };
      getLocalData();
    } catch (error) {
      console.log(error);
    }
  }, []);

  // useEffect(() => {
  //   console.log("User ID: ", userId);
  // }, [userId]);

  const login = (user: loginUser, token: string) => {
    const stringUser = JSON.stringify(user);
    let currentDate = new Date();
    localStorage.setItem("user", stringUser);
    localStorage.setItem("token", token);
    localStorage.setItem(
      "expiration",
      currentDate.setHours(currentDate.getHours() + 1).toString()
    );
    console.log(currentDate.setHours(currentDate.getHours() + 1).toString())
    setUserAccountName(user?.accountName);
    setUserRole(user?.roleId);
    setUserId(user?.id);
    setToken(token);
  };

  const logout = () => {
    setUserAccountName(undefined);
    setUserRole(undefined);
    setUserId(undefined);
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
    <UserContext.Provider
      value={{
        userAccountName,
        userId,
        userRole,
        token,
        login,
        logout,
        isAuth,
      }}
    >
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;
