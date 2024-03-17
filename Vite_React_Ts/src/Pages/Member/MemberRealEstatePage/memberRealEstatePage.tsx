import { useContext, useEffect, useState } from "react";
import RealEstateList from "../../../components/RealEstate/realEstateList";
import { getMemberRealEstates } from "../../../api/memberRealEstate";
import { UserContext } from "../../../context/userContext";
import realEstate from "../../../interface/RealEstate/realEstate";

const MemberRealEstatePage = () => {
  const { token } = useContext(UserContext);
  const [realEstateList, setRealEstateList] = useState<
    realEstate[] | undefined
  >([]);

  useEffect(() => {
    try {
      const fetchRealEstates = async () => {
        if (token) {
          const response = await getMemberRealEstates(token);
          if (response) {
            setRealEstateList(response);
          }
        }
      };
      fetchRealEstates();
    } catch (error) {
      console.log(error);
    }
  }, []);

  return (
    <>
      <div className="pt-20">
        <div className="container w-full mx-auto">
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              Your Real Estate
            </div>
            <div className="mt-2">
              View all of your real estate and their statuses
            </div>
          </div>
          <RealEstateList
            realEstatesList={realEstateList}
            ownRealEstates={true}
          />
        </div>
      </div>
    </>
  );
};

export default MemberRealEstatePage;
