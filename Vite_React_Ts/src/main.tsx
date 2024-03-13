import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import { ThemeProvider } from "@material-tailwind/react";
import { GoogleOAuthProvider } from "@react-oauth/google";
import UserProvider from "./context/userContext.tsx";
import { Toaster } from "react-hot-toast";
import DepositProvider from "./context/depositContext.tsx";

const clientId = process.env.REACT_APP_GOOGLE_CLIENT_ID || "";
ReactDOM.createRoot(document.getElementById("root")!).render(
  <ThemeProvider>
    <GoogleOAuthProvider clientId={clientId}>
      <UserProvider>
        <DepositProvider>
          <Toaster position="top-right" reverseOrder={true} />
          <App />
        </DepositProvider>
      </UserProvider>
    </GoogleOAuthProvider>
  </ThemeProvider>
);
