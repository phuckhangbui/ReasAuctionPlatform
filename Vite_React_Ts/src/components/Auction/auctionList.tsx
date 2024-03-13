import React, { useEffect, useState } from "react";
import realEstate from "../../interface/RealEstate/realEstate";
import RealEstateDetailModal from "../RealEstateDetailModal/realEstateDetailModal";
import AuctionCard from "./auctionCard";

interface RealEstateListProps {
  realEstatesList?: realEstate[];
}

const AuctionList = ({ realEstatesList = [] }: RealEstateListProps) => {
  const [realEstates] = useState(realEstatesList);
  const [showModal, setShowModal] = useState(false);
  const [realEstateId, setRealEstateId] = useState<number>(-1);

  const toggleModal = (realEstateId: number) => {
    setShowModal((prevShowModal) => !prevShowModal);
    setRealEstateId(realEstateId);
  };

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

  return (
    <div>
      <div>
        <div className="mt-4 grid xl:grid-cols-5 lg:grid-cols-4 md:grid-cols-3 md:gap-3 sm:grid-cols-1">
          {realEstates.map((realEstate) => (
            <div
              key={realEstate.reasId}
              onClick={() => toggleModal(realEstate.reasId)}
            >
              <AuctionCard realEstate={realEstate} />
            </div>
          ))}
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
            address="1600 Amphitheatre Parkway, Mountain View, CA"
            index="auction"
          />
        </div>
      )}
    </div>
  );
};

export default AuctionList;
