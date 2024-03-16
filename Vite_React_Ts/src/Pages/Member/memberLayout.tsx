import { Outlet } from "react-router-dom";
import Footer from "../../components/Footer/footer";
import Header from "../../components/Header/header";

const MemberLayout = () => {
  return (
    <div>
      <Header />
      <Outlet />
      <Footer />
    </div>
  );
};

export default MemberLayout;
