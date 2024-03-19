import { ReactNode, createContext, useEffect, useState } from "react";

interface DepositProviderProps {
  children: ReactNode;
}
interface DepositContextType {
  depositId: number | undefined;
  getDeposit: (id: number) => void;
  removeDeposit: () => void;
}

export const DepositContext = createContext<DepositContextType>({
  depositId: undefined,
  getDeposit: () => {},
  removeDeposit: () => {},
});

const DepositProvider = ({ children }: DepositProviderProps) => {
  const [depositId, setDepositId] = useState<number | undefined>();

  useEffect(() => {
    try {
      const getDepositId = async () => {
        const storageId = localStorage.getItem("depositId");
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
    console.log("Deposit:", depositId);
  }, [depositId]);

  const getDeposit = (id: number) => {
    localStorage.setItem("depositId", id.toString());
    setDepositId(id);
  };

  const removeDeposit = () => {
    localStorage.removeItem("depositId");
    setDepositId(undefined);
  };

  return (
    <DepositContext.Provider value={{ depositId, getDeposit, removeDeposit }}>
      {children}
    </DepositContext.Provider>
  );
};
export default DepositProvider;
