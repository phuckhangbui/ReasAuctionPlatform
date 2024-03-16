import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import PageNotFound from "./Pages/PageNotFound";
import { AdminLayout } from "./Pages/Admin/AdminLayout";
import AdminDashboard from "./Pages/Admin/AdminDashboard";
import AuctionOngoing from "./Pages/Admin/AdminAuctionOngoing";
import AdminStaffList from "../src/Pages/Admin/StaffList/StaffList/index";
import AdminMemberList from "../src/Pages/Admin/MemberList/MemberList/index";
import AdminAddStaff from "../src/Pages/Admin/AdminCreateStaff/AdminCreateStaff";
import PendingList from "../src/Pages/Admin/AdminRealEstatePending";
import AllList from "../src/Pages/Admin/AdminRealEstateAll";
import AuctionComplete from "../src/Pages/Admin/AdminAuctionComplete";
import HomePage from "./Pages/Member/HomePage/homePage";
import RealEstatePage from "./Pages/Member/RealEstatePage/realEstatePage";
import HelpPage from "./Pages/Member/HelpPage/helpPage";
import MemberLayout from "./Pages/Member/memberLayout";
import AuctionPage from "./Pages/Member/AuctionPage/auctionPage";
import NewsPage from "./Pages/Member/NewsPage/newsPage";
import RequiredAuth from "./components/RequiredAuth/requiredAuth";
import SellPage from "./Pages/Member/SellPage/sellPage";
import AllTransaction from "./Pages/Admin/AdminTransaction";
import AllDeposit from "./Pages/Admin/AdminDeposit";
import Task from "./Pages/Admin/AdminTask";
import TaskCreate from "./Pages/Admin/AdminTaskCreate";
import AdminAddNews from "./Pages/Admin/AdminCreateNews/AdminCreateNews";
import AdminNewsList from "./Pages/Admin/AdminNews/AdminNews";
import { useEffect } from "react";
import { onMessage } from "firebase/messaging";
import { messaging } from "./Config/firebase-config";
import toast from "react-hot-toast";
import AuctionHistory from "./Pages/Member/AuctionHistory/AuctionHistory";
import SuccessPage from "./Pages/Member/SuccessPage/successPage";
import MemberRealEstatePage from "./Pages/Member/MemberRealEstatePage/memberRealEstatePage";
import AdminCreateNews from "./Pages/Admin/AdminCreateNews";
import NewsList from "./components/News/newsList";
import DepositList from "./Pages/Admin/AdminCreateAuction";
import AddRule from "./Pages/Admin/AdminAddRule";
import AdminRule from "./Pages/Admin/AdminRule";

const roles = {
  Admin: 1,
  Staff: 2,
  Member: 3,
};

function App() {
  useEffect(() => {
    // generateToken();
    onMessage(messaging, (payload) => {
      console.log(payload);
      if (payload.notification?.body) {
        toast.success(payload.notification?.body);
      }
    });
  }, []);
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<MemberLayout />}>
            <Route index element={<HomePage />} />
            <Route path="/realEstate" element={<RealEstatePage />} />
            <Route path="/auction" element={<AuctionPage />} />
            <Route path="/help" element={<HelpPage />} />
            <Route path="/news" element={<NewsPage />} />

            <Route element={<RequiredAuth allowedRoles={[roles.Member]} />}>
              <Route path="/sell" element={<SellPage />} />
              <Route path="/history" element={<AuctionHistory />} />
              <Route path="/memberReas" element={<MemberRealEstatePage />} />
              <Route path="/success" element={<SuccessPage />} />
            </Route>
          </Route>

          <Route
            element={<RequiredAuth allowedRoles={[roles.Admin, roles.Staff]} />}
          >
            <Route path="/admin" element={<AdminLayout />}>
              <Route index element={<AdminDashboard />} />

              <Route element={<RequiredAuth allowedRoles={[roles.Admin]} />}>
              <Route path="term" element={<AdminRule/>}/>
              <Route path="term/create" element={<AddRule/>}/>

                <Route path="task" element={<Task />} />
                <Route path="task/create" element={<TaskCreate />} />
              </Route>

              <Route path="news" element={<NewsList/>}/>
              <Route path="news/create" element={<AdminCreateNews/>}/>

              <Route path="auction/ongoing" element={<AuctionOngoing />} />
              <Route path="auction/complete" element={<AuctionComplete />} />
              <Route path= "auction/create" element ={<DepositList/>}/>

              <Route path="user/staff" element={<AdminStaffList />} />
              <Route path="user/member" element={<AdminMemberList />} />
              <Route path="user/create" element={<AdminAddStaff />} />

              <Route path="news" element={<AdminNewsList />} />
              <Route path="news/create" element={<AdminAddNews />} />

              <Route path="real-estate/pending" element={<PendingList />} />
              <Route path="real-estate/all" element={<AllList />} />

              <Route path="transaction" element={<AllTransaction />} />

              <Route path="deposit" element={<AllDeposit />} />
            </Route>
          </Route>

          <Route path="/unauthorized" element={<PageNotFound />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
