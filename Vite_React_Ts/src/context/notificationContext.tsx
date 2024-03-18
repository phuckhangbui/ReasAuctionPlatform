import { ReactNode, createContext, useState } from "react";

interface NotificationProviderProps {
  children: ReactNode;
}

interface NotificationContextType {
  hasNewNoti: boolean;
  updateNotiStatus: (newStatus: boolean) => void;
}

export const NotificationContext = createContext<NotificationContextType>({
  hasNewNoti: false,
  updateNotiStatus: () => {},
});

const NotificationProvider = ({ children }: NotificationProviderProps) => {
  const [hasNewNoti, setHasNewNoti] = useState<boolean>(false);

  const updateNotiStatus = (newStatus: boolean) => {
    setHasNewNoti(newStatus);
  };

  return (
    <NotificationContext.Provider value={{ hasNewNoti, updateNotiStatus }}>
      {children}
    </NotificationContext.Provider>
  );
};

export default NotificationProvider;
