import { useEffect } from "react";

const MemberRealEstatePage = () => {
  useEffect(() => {
    try {
      const fetchRealEstates = async () => {
        // const response = await getRealEstateHome();
        // setRealEstateList(response);
      };
      fetchRealEstates();
    } catch (error) {
      console.log(error);
    }
  }, []);

  return <div>MemberRealEstatePage</div>;
};

export default MemberRealEstatePage;
