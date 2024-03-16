import { Link } from "react-router-dom";

const Footer = () => {
  return (
    <footer className="bg-white shadow mt-4 xs:mt-8">
      <div className="w-full max-w-screen-xl mx-auto p-4 md:py-8">
        <hr className="my-6 border-gray-200 sm:mx-auto lg:my-8" />
        <span className="block text-sm text-gray-900 sm:text-center xs:text-center">
          © 2023{" "}
          <Link to={"/"} className="hover:underline">
            REAS™
          </Link>
          . All Rights Reserved.
        </span>
      </div>
    </footer>
  );
};

export default Footer;
