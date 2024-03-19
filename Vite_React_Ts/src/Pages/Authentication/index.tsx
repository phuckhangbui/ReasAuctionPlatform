// import { auth, provider } from "../../config/firebase-config";
// import { signInWithPopup } from "firebase/auth";
// import { useNavigate } from "react-router-dom";
// import "./style.css";
// import { Button } from "@material-tailwind/react";

// export const Auth = () => {
//   const navigate = useNavigate();

//   const signInWithGoogle = async () => {
//     const result = await signInWithPopup(auth, provider);

//     const authInfo = {
//       name: result.user.displayName,
//       profilePhoto: result.user.photoURL,
//       userID: result.user.uid,
//       isAuth: true,
//     };
//     localStorage.setItem("auth", JSON.stringify(authInfo));

//     navigate("/admin");
//   };

//   return (
//     <div className="login-page">
//       <Button
//         onClick={signInWithGoogle}
//         size="lg"
//         color="white"
//         className="flex items-center gap-3"
//       >
//         <img
//           src="https://docs.material-tailwind.com/icons/google.svg"
//           alt="metamask"
//           className="h-6 w-6"
//         />
//         Continue with Google
//       </Button>
//       {/* <button className="login-with-google-btn" onClick={signInWithGoogle}>
//         {""}
//         Sign In with Google
//       </button> */}
//     </div>
//   );
// };
