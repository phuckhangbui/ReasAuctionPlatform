import { useEffect, useState } from "react";
import NewsCard from "./newsCard";
import NewsDetailModal from "../NewsDetailModal/newsDetailModal";

interface NewsListProp {
  newsList?: news[];
}

const NewsList = ({ newsList }: NewsListProp) => {
  const [news, setNews] = useState<news[] | undefined>([]);
  const [showModal, setShowModal] = useState(false);
  const [newsId, setNewsId] = useState<number>(0);

  const toggleModal = (newsId: number) => {
    setShowModal((prevShowModal) => !prevShowModal);
    setNewsId(newsId);
  };
  useEffect(() => {
    if (newsList) {
      setNews(newsList);
    }
  }, [newsList]);

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
        <div className="mt-4 grid lg:grid-cols-2 md:grid-cols-2 md:gap-3 sm:grid-cols-1">
          {news &&
            news.map((news) => (
              <div key={news.newsId} onClick={() => toggleModal(news.newsId)}>
                <NewsCard news={news} />
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
          <NewsDetailModal closeModal={closeModal} newsId={newsId} />
        </div>
      )}
    </div>
  );
};

export default NewsList;
