import { ReactNode, createContext, useEffect, useState } from "react";

interface DepositProviderProps {
  children: ReactNode;
}
interface DepositContextType {
  depositId: number | undefined;
  getDeposit: (id: number) => void;
}

export const DepositContext = createContext<DepositContextType>({
  depositId: undefined,
  getDeposit: () => {},
});

const DepositProvider = ({ children }: DepositProviderProps) => {
  const [depositId, setDepositId] = useState<number | undefined>();

  useEffect(() => {
    try {
      const getDepositId = async () => {
        const storageId = localStorage.getItem("id");
        if (storageId) {
          setDepositId(parseInt(storageId));
        }
      };
      getDepositId();
    } catch (error) {
      console.log("Error:", error);
    }
  }, []);

  useEffect(() => {
    console.log("depositId:", depositId);
  }, [depositId]);

  const getDeposit = (id: number) => {
    localStorage.setItem("id", id.toString());
    setDepositId(id);
  };

  return (
    <DepositContext.Provider value={{ depositId, getDeposit }}>
      {children}
    </DepositContext.Provider>
  );
};
export default DepositProvider;
