import { Link } from "react-router-dom";

const PageNotFound: React.FC = () => {
  return (
    <div className="flex items-center justify-center h-screen bg-gray-100">
      <div className="text-center">
        <h1 className="text-6xl font-semibold text-gray-900">404</h1>
        <p className="text-gray-600 mt-2">Page not found</p>
        <p className="mt-4 text-gray-600">
          The page you're looking for does not seem to exist
        </p>
        <Link
          to="/home"
          className="block mx-auto my-8 px-4 py-2 bg-primary rounded hover:bg-red-700"
        >
          Go Home
        </Link>
      </div>
    </div>
  );
};

export default PageNotFound;
