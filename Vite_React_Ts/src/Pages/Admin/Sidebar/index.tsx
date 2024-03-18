import React, { useState, useContext } from "react";
import {
  DesktopOutlined,
  PieChartOutlined,
  HomeOutlined,
  UserOutlined,
  MenuUnfoldOutlined,
  MenuFoldOutlined,
  GlobalOutlined,
  BookOutlined,
  DollarOutlined,
  SnippetsOutlined,
} from "@ant-design/icons";
import type { MenuProps } from "antd";
import { Button, Col, Layout, Menu, Row, theme } from "antd";
import { Outlet, useNavigate } from "react-router-dom";
import { UserContext } from "../../../context/userContext";

const { Header, Content, Footer, Sider } = Layout;

type MenuItem = Required<MenuProps>["items"][number];

function getItem(
  label: React.ReactNode,
  key: React.Key,
  icon?: React.ReactNode,
  children?: MenuItem[]
): MenuItem {
  return {
    key,
    icon,
    children,
    label,
  } as MenuItem;
}

const Sidebar: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const { logout } = useContext(UserContext);
  const {
    token: { colorBgContainer },
  } = theme.useToken();
  const { userRole } = useContext(UserContext);

  const navigate = useNavigate();

  const items: MenuItem[] = [
    userRole === 1
      ? getItem("Dashboard", "1", <PieChartOutlined />)
      : undefined,
    getItem("Auction", "sub1", <DesktopOutlined />, [
      getItem("InProgress Auction", "2"),
      getItem("Completed Auction", "3"),
      getItem("Create Auction", "4"),
    ]),
    userRole === 1
      ? getItem("User", "sub2", <UserOutlined />, [
          getItem("Staffs", "5"),
          getItem("Members", "6"),
          getItem("Create New Staff", "7"),
        ])
      : getItem("Members", "6", <UserOutlined />),
    getItem("Real Estate", "sub3", <HomeOutlined />, [
      getItem("All Real Estates", "8"),
      getItem("Pending Real Estate", "9"),
    ]),
    getItem("News", "sub4", <GlobalOutlined />, [
      getItem("News", "10"),
      getItem("Add News", "11"),
    ]),
    userRole === 1
      ? getItem("Term", "sub5", <BookOutlined />, [
          getItem("Term", "12"),
          getItem("Add Term", "13"),
        ])
      : undefined,

    getItem("Transaction", "14", <DollarOutlined />),
    getItem("Deposit", "15", <PieChartOutlined />),
    getItem("Logout", "16"),
  ].filter(Boolean) as MenuItem[];

  const handleMenuClick = (key: React.Key) => {
    switch (key) {
      case "1":
        navigate("/admin");
        break;
      case "2":
        navigate("/admin/auction/ongoing");
        break;
      case "3":
        navigate("/admin/auction/complete");
        break;
      case "4":
        navigate("/admin/auction/create");
        break;
      case "5":
        navigate("/admin/user/staff");
        break;
      case "6":
        navigate("/admin/user/member");
        break;
      case "7":
        navigate("/admin/user/create");
        break;
      case "8":
        navigate("/admin/real-estate/all");
        break;
      case "9":
        navigate("/admin/real-estate/pending");
        break;
      case "10":
        navigate("/admin/news");
        break;
      case "11":
        navigate("/admin/news/create");
        break;
      case "12":
        navigate("/admin/term");
        break;
      case "13":
        navigate("/admin/term/create");
        break;
      case "14":
        navigate("/admin/transaction");
        break;
      case "15":
        navigate("/admin/deposit");
        break;
      case "16":
        logout();
        navigate("/");
        break;
      default:
        break;
    }
  };
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Sider collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
        <div className="demo-logo-vertical" />
        <Menu
          theme="dark"
          defaultSelectedKeys={["1"]}
          mode="inline"
          items={items}
          onClick={({ key }) => handleMenuClick(key)}
        />
      </Sider>
      <Layout>
        <Header style={{ padding: 0, background: colorBgContainer }}>
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
            style={{
              fontSize: "16px",
              width: 64,
              height: 64,
            }}
          />
        </Header>
        <Content className="w-full">
          <Row
            gutter={[16, 16]}
            style={{
              margin: 0,
            }}
          >
            <Col span={24}>
              <main>
                <Outlet />
              </main>
            </Col>
          </Row>
        </Content>
        <Footer style={{ textAlign: "center" }}>
          Ant Design ©{new Date().getFullYear()} Created by Ant UED
        </Footer>
      </Layout>
    </Layout>
  );
};

export default Sidebar;
