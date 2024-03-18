import { Card } from "antd";
import ListBidder from "../ListBidder";
import "./style.css";
const gridStyle: React.CSSProperties = {
  width: "100%",
  textAlign: "center",
};

const Bidder: React.FC = () => {
  // const [activeTabKey, setActiveTabKey] = useState("week");

  // const handleTabChange = (key: string) => {
  //   setActiveTabKey(key);
  //   console.log(key);
  // };

  return (
    <>
      <Card
        title={
          <span
            style={{
              color: "#bdc3c9",
              fontSize: "30px",
              fontWeight: "bold",
            }}
          >
            Bidder
          </span>
        }
      >
        <Card.Grid
          style={{ width: "100%", textAlign: "center", padding: 0 }}
          hoverable={false}
        >
        </Card.Grid>
        <Card.Grid style={gridStyle} hoverable={false}>
          <ListBidder />
        </Card.Grid>
      </Card>
    </>
  );
};

export default Bidder;
