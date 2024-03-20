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
import { useContext, useEffect } from "react";
import { onMessage } from "firebase/messaging";
import { generateToken, messaging } from "./Config/firebase-config";
import toast from "react-hot-toast";
import SuccessPage from "./Pages/Member/SuccessPage/successPage";
import MemberRealEstatePage from "./Pages/Member/MemberRealEstatePage/memberRealEstatePage";
import DepositList from "./Pages/Admin/AdminCreateAuction";
import AddRule from "./Pages/Admin/AdminAddRule";
import AdminRule from "./Pages/Admin/AdminRule";
import AuctionHistory from "./Pages/Member/AuctionHistory/AuctionHistory";
import TransactionHistory from "./Pages/Member/TransactionHistory";
import ProfilePage from "./Pages/Member/ProfilePage/profilePage";
import UpdateRealEstatePage from "./Pages/Member/UpdateRealEstatePage/updateRealEstatePage";
import { NotificationContext } from "./context/notificationContext";
import { UserContext } from "./context/userContext";

const roles = {
  Admin: 1,
  Staff: 2,
  Member: 3,
};

function App() {
  const { updateNotiStatus } = useContext(NotificationContext);
  const { userId } = useContext(UserContext);

  const iconChooser = (status: string) => {
    console.log(status);
    switch (status) {
      case "1": // NewRealEstateCreate
        return (
          <div className="rounded-full bg-greenSuccess w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="m4.5 12.75 6 6 9-13.5"
              />
            </svg>
          </div>
        );
      case "2": // NewRealEstateApproved
        return (
          <div className="rounded-full bg-greenSuccess w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="m4.5 12.75 6 6 9-13.5"
              />
            </svg>
          </div>
        );
      case "3": // NewRealEstateRejected
        return (
          <div className="rounded-full bg-redFailed w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M6 18 18 6M6 6l12 12"
              />
            </svg>
          </div>
        );
      case "4": // NewAuctionCreate
        return (
          <div className="rounded-full bg-mainBlue w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M10.34 15.84c-.688-.06-1.386-.09-2.09-.09H7.5a4.5 4.5 0 1 1 0-9h.75c.704 0 1.402-.03 2.09-.09m0 9.18c.253.962.584 1.892.985 2.783.247.55.06 1.21-.463 1.511l-.657.38c-.551.318-1.26.117-1.527-.461a20.845 20.845 0 0 1-1.44-4.282m3.102.069a18.03 18.03 0 0 1-.59-4.59c0-1.586.205-3.124.59-4.59m0 9.18a23.848 23.848 0 0 1 8.835 2.535M10.34 6.66a23.847 23.847 0 0 0 8.835-2.535m0 0A23.74 23.74 0 0 0 18.795 3m.38 1.125a23.91 23.91 0 0 1 1.014 5.395m-1.014 8.855c-.118.38-.245.754-.38 1.125m.38-1.125a23.91 23.91 0 0 0 1.014-5.395m0-3.46c.495.413.811 1.035.811 1.73 0 .695-.316 1.317-.811 1.73m0-3.46a24.347 24.347 0 0 1 0 3.46"
              />
            </svg>
          </div>
        );
      case "5": // AuctionAboutToStart
        return (
          <div className="rounded-full bg-yellowWinner w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M12 6v6h4.5m4.5 0a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z"
              />
            </svg>
          </div>
        );
      case "6": // AuctionFinishWinner
        return (
          <div className="rounded-full bg-yellowWinner w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M16.5 18.75h-9m9 0a3 3 0 0 1 3 3h-15a3 3 0 0 1 3-3m9 0v-3.375c0-.621-.503-1.125-1.125-1.125h-.871M7.5 18.75v-3.375c0-.621.504-1.125 1.125-1.125h.872m5.007 0H9.497m5.007 0a7.454 7.454 0 0 1-.982-3.172M9.497 14.25a7.454 7.454 0 0 0 .981-3.172M5.25 4.236c-.982.143-1.954.317-2.916.52A6.003 6.003 0 0 0 7.73 9.728M5.25 4.236V4.5c0 2.108.966 3.99 2.48 5.228M5.25 4.236V2.721C7.456 2.41 9.71 2.25 12 2.25c2.291 0 4.545.16 6.75.47v1.516M7.73 9.728a6.726 6.726 0 0 0 2.748 1.35m8.272-6.842V4.5c0 2.108-.966 3.99-2.48 5.228m2.48-5.492a46.32 46.32 0 0 1 2.916.52 6.003 6.003 0 0 1-5.395 4.972m0 0a6.726 6.726 0 0 1-2.749 1.35m0 0a6.772 6.772 0 0 1-3.044 0"
              />
            </svg>
          </div>
        );
      case "7": // AuctionFinishLoser
        return (
          <div className="rounded-full bg-redFailed w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M15.182 16.318A4.486 4.486 0 0 0 12.016 15a4.486 4.486 0 0 0-3.198 1.318M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0ZM9.75 9.75c0 .414-.168.75-.375.75S9 10.164 9 9.75 9.168 9 9.375 9s.375.336.375.75Zm-.375 0h.008v.015h-.008V9.75Zm5.625 0c0 .414-.168.75-.375.75s-.375-.336-.375-.75.168-.75.375-.75.375.336.375.75Zm-.375 0h.008v.015h-.008V9.75Z"
              />
            </svg>
          </div>
        );
      case "8": // AuctionFinishNotAttender
        return (
          <div className="rounded-full bg-redFailed w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M22 10.5h-6m-2.25-4.125a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0ZM4 19.235v-.11a6.375 6.375 0 0 1 12.75 0v.109A12.318 12.318 0 0 1 10.374 21c-2.331 0-4.512-.645-6.374-1.766Z"
              />
            </svg>
          </div>
        );
      case "9": // AuctionFinishAdminAndStaff
        return (
          <div className="rounded-full bg-mainBlue w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M10.34 15.84c-.688-.06-1.386-.09-2.09-.09H7.5a4.5 4.5 0 1 1 0-9h.75c.704 0 1.402-.03 2.09-.09m0 9.18c.253.962.584 1.892.985 2.783.247.55.06 1.21-.463 1.511l-.657.38c-.551.318-1.26.117-1.527-.461a20.845 20.845 0 0 1-1.44-4.282m3.102.069a18.03 18.03 0 0 1-.59-4.59c0-1.586.205-3.124.59-4.59m0 9.18a23.848 23.848 0 0 1 8.835 2.535M10.34 6.66a23.847 23.847 0 0 0 8.835-2.535m0 0A23.74 23.74 0 0 0 18.795 3m.38 1.125a23.91 23.91 0 0 1 1.014 5.395m-1.014 8.855c-.118.38-.245.754-.38 1.125m.38-1.125a23.91 23.91 0 0 0 1.014-5.395m0-3.46c.495.413.811 1.035.811 1.73 0 .695-.316 1.317-.811 1.73m0-3.46a24.347 24.347 0 0 1 0 3.46"
              />
            </svg>
          </div>
        );
      case "10": // RealEstateStatusChangeNotiToOwner
        return (
          <div className="rounded-full bg-mainBlue w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M12 9v3.75m9-.75a9 9 0 1 1-18 0 9 9 0 0 1 18 0Zm-9 3.75h.008v.008H12v-.008Z"
              />
            </svg>
          </div>
        );
      case "11": // AuctionWinnerNoContact
        return (
          <div className="rounded-full bg-yellowWinner w-10 h-10 flex justify-center items-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="1.5"
              stroke="currentColor"
              className="w-8 h-8 text-white font-extrabold"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z"
              />
            </svg>
          </div>
        );
    }
  };

  useEffect(() => {
    generateToken();
    onMessage(messaging, (payload) => {
      updateNotiStatus(true);
      console.log(payload);
      if (payload.notification?.body) {
        const { title, body } = payload.notification;
        let status: string;
        let accountId: number;
        console.log("Got in 1")
        if (payload.data) {
          status = payload.data.type;
          accountId = parseInt(payload.data.accountId);
          console.log(accountId);
          if (accountId === userId) {
            toast.custom((t) => (
              <div
                className={`${
                  t.visible ? "animate-enter" : "animate-leave"
                } max-w-sm w-full bg-white shadow-lg rounded-lg pointer-events-auto flex ring-1 ring-black ring-opacity-5`}
              >
                <div className="flex-1 w-0 p-1.5">
                  <div className="flex items-center justify-center">
                    <div className="flex-shrink-0 pl-1">
                      {iconChooser(status)}
                    </div>
                    <div className="ml-3 flex-1">
                      <p className="text-lg font-medium text-gray-900">
                        {title}
                      </p>
                      <p className=" text-sm text-gray-500">{body}</p>
                    </div>
                  </div>
                </div>
              </div>
            ));
          }
        }
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
              <Route path="/history" element={<AuctionHistory />} />
              <Route path="/transaction" element={<TransactionHistory />} />
              <Route path="/sell" element={<SellPage />} />
              <Route path="/memberReas" element={<MemberRealEstatePage />} />
              <Route
                path="/update/:reasId"
                element={<UpdateRealEstatePage />}
              />
              <Route path="/success" element={<SuccessPage />} />
              <Route path="/profile" element={<ProfilePage />} />
            </Route>
          </Route>

          <Route
            element={<RequiredAuth allowedRoles={[roles.Admin, roles.Staff]} />}
          >
            <Route path="/admin" element={<AdminLayout />}>
              <Route index element={<AdminDashboard />} />

              <Route element={<RequiredAuth allowedRoles={[roles.Admin]} />}>
                <Route path="term" element={<AdminRule />} />
                <Route path="term/create" element={<AddRule />} />

                <Route path="task" element={<Task />} />
                <Route path="task/create" element={<TaskCreate />} />
              </Route>

              <Route path="auction/ongoing" element={<AuctionOngoing />} />
              <Route path="auction/complete" element={<AuctionComplete />} />
              <Route path="auction/create" element={<DepositList />} />

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
