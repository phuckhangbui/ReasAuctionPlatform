import React from "react";
import { Card, Col, Row } from "antd";
import Summary from "./DashboardSummary";
import Total from "./RamdomAuction";
import Bidder from "./BidderSummary";
import DemoLine from "./ProfitChart";
import PropertiesPie from "./PieChart";

const AdminDashboard: React.FC = () => (
  <>
    <Summary />

    <Card>
      <Card.Grid
        style={{ width: "66.666%", textAlign: "center", padding: 0 }}
        hoverable={false}
      >
        <Total />
      </Card.Grid>
      <Card.Grid
        style={{ width: "33.333%", textAlign: "center", padding: 0 }}
        hoverable={false}
      >
        <Bidder />
      </Card.Grid>
    </Card>
    <Row>
      <Col span={16}>
        <DemoLine />
      </Col>
      <Col span={8}>
        <PropertiesPie />
      </Col>
    </Row>
  </>
);

export default AdminDashboard;
