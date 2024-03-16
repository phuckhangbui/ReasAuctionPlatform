import { Pie } from "@ant-design/plots";
import { useState, useEffect , useContext} from "react";
import {
  getTypeReal
} from "../../../../api/admindashboard";
import { UserContext } from "../../../../context/userContext";


const PropertiesPie: React.FC = () => {
  const { token } = useContext(UserContext);
  const [typeData, settypeData] = useState<TypeReas[]| undefined>(undefined);
  
  useEffect(() => {
    const fetchGetTypeReas = async () => {
      try {
        if (token) {
          const data: TypeReas[] | undefined = await getTypeReal(token);
          settypeData(data);
        }
      } catch (error) {
        console.error("Error fetching type list:", error);
      }
    };

    fetchGetTypeReas();
  }, [token]); // Thêm token vào dependencies của useEffect

  // Nếu dữ liệu chưa được tải, hiển thị một thông báo hoặc một loại loading indicator
  if (!typeData) {
    return <div>Loading...</div>;
  }

  const config = {
    appendPadding: 10,
    data: typeData,
    angleField: "numberOfReas", 
    colorField: "typeName", 
    radius: 0.8,
    label: {
      type: "outer",
    },
    interactions: [
      {
        type: "element-active",
      },
    ],
  };

  return <Pie {...config} />;
};

export default PropertiesPie;
