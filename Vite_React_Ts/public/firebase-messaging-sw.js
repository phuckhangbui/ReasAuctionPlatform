importScripts("https://www.gstatic.com/firebasejs/8.10.1/firebase-app.js");
importScripts(
  "https://www.gstatic.com/firebasejs/8.10.1/firebase-messaging.js"
);

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
firebase.initializeApp(firebaseConfig);

const messaging = firebase.messaging();
messaging.onBackgroundMessage((payload) => {
  console.log("Received background message: ", payload);

  const notificationTitle = payload.notification?.title;
  const notificationOptions = {
    body: payload.notification?.body,
    icon: "../public/REAS-removebg-preview.png",
  };
  return self.registration.showNotification(
    notificationTitle,
    notificationOptions
  );
});
