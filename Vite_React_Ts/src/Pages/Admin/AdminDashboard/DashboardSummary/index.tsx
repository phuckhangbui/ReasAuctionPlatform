import { Card, Statistic } from "antd";
import { NumberFormat } from "../../../../Utils/numberFormat";
import {
  DollarOutlined,
  UserOutlined,
} from "@ant-design/icons";
import { useState, useEffect , useContext} from "react";
import {
  getTotalRenueve,
  getStaffActive
} from "../../../../api/admindashboard";
import { UserContext } from "../../../../context/userContext";

const gridStyle: React.CSSProperties = {
  width: "33.333%",
  textAlign: "center",
  //   borderStyle: "none",
};

const Summary: React.FC = () => {
  const { token } = useContext(UserContext);
  const [totalData, settotalData] = useState<number| undefined>();
  const [staffData, setStaffData] = useState<number| undefined>();
  
  useEffect(() => {
    const fetchGetTotalRevenue = async () => {
      try {
        if (token) {
          const data: number | undefined = await getTotalRenueve(token);
          settotalData(data);
        }
      } catch (error) {
        console.error("Error fetching total revenue:", error);
      }
    };

    fetchGetTotalRevenue();
  }, [token]);

  useEffect(() => {
    const fectchCountStaff = async () => {
      try {
        if (token) {
          const data: number | undefined = await getStaffActive(token);
          setStaffData(data);
        }
      } catch (error) {
        console.error("Error fetching total staff:", error);
      }
    };

    fectchCountStaff();
  }, [token]);
  return (
    <>
      <Card>
        <Card.Grid style={gridStyle} hoverable={false}>
          <Statistic
            title={
              <span
                style={{
                  color: "#bdc3c9",
                  fontSize: "30px",
                  fontWeight: "bold",
                }}
              >
                TOTAL REVENUE
              </span>
            }
            value={NumberFormat(totalData || 0)}
            valueStyle={{ color: "#001529", fontWeight: "bold" }}
            prefix={
              <span
                style={{ color: "red", fontSize: "30px", fontWeight: "bold" }}
              >
                <DollarOutlined />
              </span>
            }
          />
        </Card.Grid>
        <Card.Grid style={gridStyle} hoverable={false}>
          <Statistic
            title={
              <span
                style={{
                  color: "#bdc3c9",
                  fontSize: "30px",
                  fontWeight: "bold",
                }}
              >
                Staff
              </span>
            }
            value={staffData}
            valueStyle={{ color: "#001529", fontWeight: "bold" }}
            prefix={
              <span
                style={{
                  color: "green",
                  fontSize: "30px",
                  fontWeight: "bold",
                }}
              >
                <UserOutlined />
              </span>
            }
          />
        </Card.Grid>
      </Card>
    </>
  );
};

export default Summary;
