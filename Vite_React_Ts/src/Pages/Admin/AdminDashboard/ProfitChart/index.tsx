import { Line } from "@ant-design/plots";
import { useState, useEffect , useContext} from "react";
import {
  getRealMonth
} from "../../../../api/admindashboard";
import { UserContext } from "../../../../context/userContext";

const DemoLine: React.FC = () => {
  const { token } = useContext(UserContext);
  const [monthData, setMonthData] = useState<ReasMonth[]| undefined>(undefined);
  
  useEffect(() => {
    const fetchGetMonthReas = async () => {
      try {
        if (token) {
          const data: ReasMonth[] | undefined = await getRealMonth(token);
          setMonthData(data);
        }
      } catch (error) {
        console.error("Error fetching month list:", error);
      }
    };

    fetchGetMonthReas();
  }, [token]); 

  if (!monthData) {
    return <div>Loading...</div>;
  }
  const config = {
    data: monthData, 
    xField: "month",
    yField: "numberOfReas",
    label: {},
    point: {
      size: 5,
      shape: 'diamond',
    },
  };
  return <Line {...config} />;
};

export default DemoLine;
