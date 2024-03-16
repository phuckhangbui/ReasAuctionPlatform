import { useEffect, useState } from "react";
import { getNewsById } from "../../api/news";

interface NewsDetailModalProps {
  newsId: number;
  closeModal: () => void;
}

const NewsDetailModal = ({ closeModal, newsId }: NewsDetailModalProps) => {
  const [newsDetail, setNewsDetail] = useState<newsDetail | undefined>();
  const [formattedDateEnd, setFormattedDateEnd] = useState<string>("");

  useEffect(() => {
    try {
      const fetchNewsDetail = async () => {
        const response = await getNewsById(newsId);
        setNewsDetail(response);
      };
      fetchNewsDetail();
      console.log(newsDetail);
    } catch (error) {
      console.log(error);
    }
  }, []);

  useEffect(() => {
    try {
      if (newsDetail?.dateCreated) {
        const dateObject = new Date(newsDetail?.dateCreated);
        const formattedDate = dateObject
          .toDateString()
          .split(" ")
          .slice(1)
          .join(" ");
        setFormattedDateEnd(formattedDate);
        console.log(formattedDate);
      }
    } catch (error) {
      console.log(error);
    }
  }, [newsDetail?.dateCreated]);

  return (
    <div className="relative w-full max-w-7xl max-h-full ">
      <div className="relative bg-white rounded-lg shadow md:px-10 md:pb-5 sm:px-0 sm:pb-0 ">
        <div className=" items-center justify-start md:py-5 md:px-0 sm:p-5 z-10 top-0">
          <button
            type="button"
            className="justify-start bg-transparent md:bg-transparent sm:bg-white sm:bg-opacity-60 rounded-3xl text-sm w-10 h-10 ms-auto inline-flex items-center "
            data-modal-hide="default-modal"
            onClick={closeModal}
          >
            <svg
              className="w-6 h-6 sm:text-black sm:hover:text-mainBlue "
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 14 10"
            >
              <path
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M13 5H1m0 0 4 4M1 5l4-4"
              />
            </svg>
            <span className="sr-only">Close modal</span>
          </button>

          <div className="">
            <div className="text-4xl p-4 font-bold">
              {newsDetail?.newsTitle}
            </div>
            {/* <div>
              <span className="text-gray-900">By: </span>
              <span className="font-bold">John Doe</span>
            </div> */}
            <div className="text-gray-500 italic">{formattedDateEnd}</div>
          </div>

          <div className="flex items-center justify-center w-full mx-auto py-2 max-w-3xl ">
            <img
              className="object-contain"
              src={newsDetail?.thumbnail}
              alt=""
            />
          </div>

          <div className="">
            <div>
            <div
                dangerouslySetInnerHTML={{
                  __html: newsDetail?.newsContent || "",
                }} className="mt-1"
              ></div>
            </div>
          </div>
        </div>
        <footer>
          <div className="w-full max-w-screen-xl">
            <hr className="mb-6 border-gray-200 sm:mx-auto lg:mb-8 " />
            <span className="block text-sm text-gray-900 sm:text-center xs:text-center sm:pb-8 md:pb-0">
              © 2023 REAS™ . All Rights Reserved.
            </span>
          </div>
        </footer>
      </div>
    </div>
  );
};

export default NewsDetailModal;
