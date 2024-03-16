import { Card } from "antd";
import { useState, useEffect , useContext} from "react";
import { getTotal, getNews } from "../../../../api/admindashboard";
import { UserContext } from "../../../../context/userContext";
import NewsCard from "../AuctionCard";

const gridStyle: React.CSSProperties = {
  width: "33.333%",
  textAlign: "center",
};

const Total: React.FC = () => {
  const { token } = useContext(UserContext);
  const [totalData, setTotalData] = useState<Total | undefined>(undefined);
  const [newsData, setNewsData] = useState<news[] | undefined>(undefined);
  
  useEffect(() => {
    const fetchData = async () => {
      try {
        if (token) {
          const total: Total | undefined = await getTotal(token);
          setTotalData(total);

          const news: news[] | undefined = await getNews(token);
          setNewsData(news);
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, [token]);

  return (
    <>
      <Card
        title={
          <div className="flex justify-between">
            <span
              style={{
                color: "burlywood",
                fontSize: "30px",
                fontWeight: "bold",
              }}
            >
              Users: {<span className="text-black">{totalData?.numberOfUser}</span>}
            </span>
            <span
              style={{
                color: "limegreen",
                fontSize: "30px",
                fontWeight: "bold",
              }}
            >
              Total Auction: {<span className="text-black">{totalData?.numberOfAuction}</span>}
            </span>
            <span
              style={{
                color: "#477ffb",
                fontSize: "30px",
                fontWeight: "bold",
              }}
            >
              Real Estate: {<span className="text-black">{totalData?.numberOfReas}</span>}
            </span>
          </div>
        }
      >
        <Card.Grid style={gridStyle} hoverable={false}>
          <NewsCard newsItem={newsData?.[0]} />
        </Card.Grid>
        <Card.Grid style={gridStyle} hoverable={false}>
          <NewsCard newsItem={newsData?.[1]} />
        </Card.Grid>
        <Card.Grid style={gridStyle} hoverable={false}>
          <NewsCard newsItem={newsData?.[2]} />
        </Card.Grid>
      </Card>
    </>
  );
};

export default Total;
