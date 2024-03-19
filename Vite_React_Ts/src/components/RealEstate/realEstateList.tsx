import React, { useEffect, useState } from "react";
import RealEstateCard from "./realEstateCard";
import RealEstateDetailModal from "../RealEstateDetailModal/realEstateDetailModal";
import realEstate from "../../interface/RealEstate/realEstate";
import { Pagination } from "antd";

interface RealEstateListProps {
  realEstatesList?: realEstate[];
  ownRealEstates?: boolean;
}

const RealEstateList = ({
  realEstatesList,
  ownRealEstates,
}: RealEstateListProps) => {
  const [realEstates, setRealEstates] = useState<realEstate[] | undefined>([]);
  const [showModal, setShowModal] = useState(false);
  const [realEstateId, setRealEstateId] = useState<number>(0);
  const [ownRealEstatesStatus, setOwnRealEstatesStatus] = useState<boolean>();
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [displayedReas, setDisplayedReas] = useState<realEstate[]>([]);

  const toggleModal = (realEstate: realEstate) => {
    setShowModal((prevShowModal) => !prevShowModal);
    setRealEstateId(realEstate.reasId);
  };
  useEffect(() => {
    if (realEstatesList) {
      setRealEstates(realEstatesList);
    }
  }, [realEstatesList]);

  useEffect(() => {
    setOwnRealEstatesStatus(ownRealEstates);
  }, [ownRealEstates]);

  useEffect(() => {
    // Disable scroll on body when modal is open
    if (showModal) {
      document.body.style.overflow = "hidden";
    } else {
      document.body.style.overflow = "auto";
    }

    // Cleanup function
    return () => {
      document.body.style.overflow = "auto";
    };
  }, [showModal]);

  const closeModal = () => {
    setShowModal(!showModal);
  };

  const handleOverlayClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) {
      // If the click occurs on the overlay (not on the modal content), close the modal
      closeModal();
    }
  };

  // Update displayed auctions when currentPage or auctionAccounting changes
  useEffect(() => {
    // Calculate the start index and end index of auctions to display based on currentPage
    const pageSize = 8; // Change this value according to your requirement
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize, realEstates?.length || 0);

    // Extract the auctions to be displayed for the current page
    const reasToDisplay = realEstates?.slice(startIndex, endIndex) || [];

    setDisplayedReas(reasToDisplay);
  }, [currentPage, realEstates]);

  // Function to handle page change
  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  return (
    <div>
      <div>
        <div className="mt-4 grid lg:grid-cols-2 md:grid-cols-1 md:gap-3 sm:grid-cols-1">
          {displayedReas &&
            displayedReas.map((realEstate) => (
              <div
                key={realEstate.reasId}
                onClick={() => toggleModal(realEstate)}
              >
                <RealEstateCard
                  realEstate={realEstate}
                  ownRealEstatesStatus={ownRealEstatesStatus}
                />
              </div>
            ))}
        </div>
        <div className="flex justify-center mt-4">
          <Pagination
            defaultCurrent={1}
            total={realEstates?.length || 0}
            pageSize={8}
            onChange={handlePageChange}
          />
        </div>
      </div>
      {showModal && (
        <div
          id="default-modal"
          tabIndex={-1}
          aria-hidden="true"
          className=" fixed top-0 left-0 right-0 inset-0 overflow-x-hidden overflow-y-auto z-50 flex items-center justify-center bg-black bg-opacity-50 w-full max-h-full md:inset-0 "
          onMouseDown={handleOverlayClick}
        >
          <RealEstateDetailModal
            closeModal={closeModal}
            realEstateId={realEstateId}
            index="detail"
          />
        </div>
      )}
    </div>
  );
};

export default RealEstateList;
