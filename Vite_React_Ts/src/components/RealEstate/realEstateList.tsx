import React, { useEffect, useState } from "react";
import RealEstateCard from "./realEstateCard";
import RealEstateDetailModal from "../RealEstateDetailModal/realEstateDetailModal";

interface RealEstateListProps {
  realEstatesList?: realEstate[];
}

const RealEstateList = ({ realEstatesList }: RealEstateListProps) => {
  const [realEstates, setRealEstates] = useState<realEstate[] | undefined>([]);
  const [showModal, setShowModal] = useState(false);
  const [realEstateId, setRealEstateId] = useState<number>(0);

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
        <div className="mt-4 grid lg:grid-cols-4 md:grid-cols-2 md:gap-3 sm:grid-cols-1">
          {realEstates &&
            realEstates.map((realEstate) => (
              <div
                key={realEstate.reasId}
                onClick={() => toggleModal(realEstate)}
              >
                <RealEstateCard realEstate={realEstate} />
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
            index="detail"
          />
        </div>
      )}
    </div>
  );
};

export default RealEstateList;
