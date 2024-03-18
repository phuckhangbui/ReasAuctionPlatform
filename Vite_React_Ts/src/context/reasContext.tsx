import { ReactNode, createContext, useEffect, useState } from "react";

interface ReasProviderProps {
  children: ReactNode;
}
interface ReasContextType {
  reasId: number | undefined;
  getReas: (id: number) => void;
  removeReas: () => void;
}

export const ReasContext = createContext<ReasContextType>({
  reasId: undefined,
  getReas: () => {},
  removeReas: () => {},
});

const ReasProvider = ({ children }: ReasProviderProps) => {
  const [reasId, setDepositId] = useState<number | undefined>();

  useEffect(() => {
    try {
      const getDepositId = async () => {
        const storageId = localStorage.getItem("reasId");
        if (storageId) {
          setDepositId(parseInt(storageId));
        }
      };
      getDepositId();
    } catch (error) {
      console.log("Error:", error);
    }
  }, []);

  // useEffect(() => {
  //   console.log("reasId:", reasId);
  // }, [reasId]);

  const getReas = (id: number) => {
    localStorage.setItem("reasId", id.toString());
    setDepositId(id);
  };

  const removeReas = () => {
    localStorage.removeItem("reasId");
    setDepositId(undefined);
  };

  return (
    <ReasContext.Provider value={{ reasId, getReas, removeReas }}>
      {children}
    </ReasContext.Provider>
  );
};
export default ReasProvider;
