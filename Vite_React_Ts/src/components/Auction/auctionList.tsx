import React, { useEffect, useState } from "react";
import RealEstateDetailModal from "../RealEstateDetailModal/realEstateDetailModal";
import AuctionCard from "./auctionCard";

interface AuctionListProps {
  auctionsList?: auction[];
}

const AuctionList = ({ auctionsList }: AuctionListProps) => {
  const [auctions, setAuctions] = useState<auction[] | undefined>([]);
  const [showModal, setShowModal] = useState(false);
  const [realEstateId, setRealEstateId] = useState<number>(0);

  const toggleModal = (realEstateId: number) => {
    setShowModal((prevShowModal) => !prevShowModal);
    setRealEstateId(realEstateId);
  };

  useEffect(() => {
    if (auctionsList) {
      setAuctions(auctionsList);
    }
  }, [auctionsList]);

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
      <div className="mt-4 grid lg:grid-cols-4 md:grid-cols-2 md:gap-3 sm:grid-cols-1">
        {auctions &&
          auctions.map((auction) => (
            <div
              key={auction.auctionId}
              onClick={() => toggleModal(auction.reasId)}
            >
              <AuctionCard auction={auction} />
            </div>
          ))}
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
            index="auction"
          />
        </div>
      )}
    </div>
  );
};

export default AuctionList;
