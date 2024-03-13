import { Card, Statistic } from "antd";
import { NumberFormat } from "../../../../Utils/numbetFormat";
import {
  DollarOutlined,
  ShoppingCartOutlined,
  RiseOutlined,
} from "@ant-design/icons";

const gridStyle: React.CSSProperties = {
  width: "33.333%",
  textAlign: "center",
  //   borderStyle: "none",
};

const Summary: React.FC = () => {
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
            value={NumberFormat(50000000)}
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
                SALES
              </span>
            }
            value={NumberFormat(40000000)}
            valueStyle={{ color: "#001529", fontWeight: "bold" }}
            prefix={
              <span
                style={{
                  color: "black",
                  fontSize: "30px",
                  fontWeight: "bold",
                }}
              >
                <ShoppingCartOutlined />
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
                PROFIT
              </span>
            }
            value={NumberFormat(15000000)}
            valueStyle={{ color: "#001529", fontWeight: "bold" }}
            prefix={
              <span
                style={{
                  color: "green",
                  fontSize: "30px",
                  fontWeight: "bold",
                }}
              >
                <RiseOutlined />
              </span>
            }
          />
        </Card.Grid>
      </Card>
    </>
  );
};

export default Summary;
