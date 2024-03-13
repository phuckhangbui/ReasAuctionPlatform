// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAuth, GoogleAuthProvider } from "firebase/auth";
import { getDatabase } from "firebase/database";
import { getMessaging, getToken } from "firebase/messaging";

// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyAM4kKUgQxULaBboGlLoknQlrG8NtbPMFo",
  authDomain: "swd-reas.firebaseapp.com",
  databaseURL:
    "https://swd-reas-default-rtdb.asia-southeast1.firebasedatabase.app",
  projectId: "swd-reas",
  storageBucket: "swd-reas.appspot.com",
  messagingSenderId: "49595114468",
  appId: "1:49595114468:web:af172b59d8948ae36a9df1",
};
//sau này có dùng thì sửa lại các attribute trên

// Initialize Firebase
const app = initializeApp(firebaseConfig);

export const auth = getAuth(app);
export const provider = new GoogleAuthProvider();
export const db = getDatabase(app);
export const messaging = getMessaging(app);

export const generateToken = async () => {
  try {
    const permission = await Notification.requestPermission();
    console.log("Notification is", permission);
    if (permission === "granted") {
      const token = await getToken(messaging, {
        vapidKey: process.env.REACT_APP_VAP_ID_KEY,
      });
      console.log("Message token:", token);
      return token;
    } else {
      const token = "";
      return token;
    }
  } catch (error) {
    console.log("An error occured while getting the token: ", error);
  }
};
