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
              Explore Our Real Estate Options
            </div>
            <div className="mt-2">
              Take a look at our various options and find your forever home
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
