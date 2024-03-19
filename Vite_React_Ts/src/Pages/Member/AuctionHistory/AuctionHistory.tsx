import {
  Card,
  CardHeader,
  CardBody,
  CardFooter,
  Typography,
  Chip,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { NumberFormat } from "../../../Utils/numbetFormat";
import { Empty, Pagination } from "antd";
import { useContext } from "react";
import { UserContext } from "../../../context/userContext";
import { GetAuctionHistory } from "../../../api/memberAuction";

const AuctionHistory: React.FC = () => {
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [displayedAuctions, setDisplayedAuctions] = useState<auctionHistory[]>(
    []
  );
  const [auctionHistory, setAuctionHistory] = useState<
    auctionHistory[] | undefined
  >([]);
  const { token } = useContext(UserContext);
  const { userId } = useContext(UserContext);

  const fetchAuctionHistory = async () => {
    if (userId && token) {
      let response = await GetAuctionHistory(userId, token);
      setAuctionHistory(response);
    }
  };

  useEffect(() => {
    try {
      fetchAuctionHistory();
    } catch (error) {
      console.log(error);
    }
  }, []);

  // Update displayed auctions when currentPage or auctionAccounting changes
  useEffect(() => {
    // Calculate the start index and end index of auctions to display based on currentPage
    const pageSize = 8; // Change this value according to your requirement
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(
      startIndex + pageSize,
      auctionHistory?.length || 0
    );

    // Extract the auctions to be displayed for the current page
    const auctionsToDisplay = auctionHistory?.slice(startIndex, endIndex) || [];

    setDisplayedAuctions(auctionsToDisplay);
  }, [currentPage, auctionHistory]);

  // Function to handle page change
  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const getColorForDepositStatus = (depositStatus: number) => {
    switch (depositStatus) {
      case 0:
        return "gray"; // Pending
      case 1:
        return "blue"; // Deposited
      case 2:
        return "yellow"; // Waiting for refund
      case 3:
        return "green"; // Refunded
      case 4:
        return "red"; // Lost deposit
      case 5:
        return "blue"; // Win auction
      default:
        return "gray";
    }
  };

  const getValueForDepositStatus = (depositStatus: number) => {
    switch (depositStatus) {
      case 0:
        return "Pending";
      case 1:
        return "Deposited";
      case 2:
        return "Waiting for refund";
      case 3:
        return "Refunded";
      case 4:
        return "Lost deposit";
      case 5:
        return "Win auction";
      default:
        return "Unknown Status";
    }
  };
  return (
    <>
      <div className="pt-20">
        <div className="container w-full mx-auto">
          <div className="mt-4 grid lg:grid-cols-4 md:grid-cols-2 md:gap-3 sm:grid-cols-1">
            {displayedAuctions && displayedAuctions.length > 0 ? (
              displayedAuctions.map((auction) => (
                <Card className="max-w-[24rem] overflow-hidden">
                  <CardHeader
                    floated={false}
                    shadow={false}
                    color="transparent"
                    className="m-0 rounded-none"
                  >
                    <img src={auction.thumbnailUrl} alt="ui/ux review check" />
                  </CardHeader>
                  <CardBody>
                    <Typography variant="h4" color="blue-gray">
                      {auction.reasName}
                    </Typography>
                    <Typography className="font-normal">
                      {/* {auction.EstimatedPaymentDate.toDateString()} */}
                      payment date
                    </Typography>
                    <div className="mb-3 font-normal text-gray-700">
                      <span className="text-gray-900 font-semibold">
                        {auction.typeName}
                      </span>
                      <span className="sm:inline md:hidden xl:inline"> | </span>
                      <br className="sm:hidden md:block xl:hidden" />
                      <span className="text-gray-900 font-semibold">
                        {auction.reasArea}
                      </span>
                      <span> sqrt</span>
                    </div>
                  </CardBody>
                  <CardFooter className="flex items-center justify-between">
                    <div className="flex items-center -space-x-3">
                      <Typography variant="h6" color="blue-gray">
                        {NumberFormat(auction.lastBid)}
                      </Typography>
                    </div>
                    {auction.depositStatus !== undefined && (
                      <Chip
                        color={getColorForDepositStatus(auction.depositStatus)}
                        value={getValueForDepositStatus(auction.depositStatus)}
                      />
                    )}
                  </CardFooter>
                </Card>
              ))
            ) : (
              <div className="py-40 col-span-4">
                <Empty />
              </div>
            )}
          </div>
          <div className="flex justify-center mt-4">
            <Pagination
              defaultCurrent={1}
              total={auctionHistory?.length || 0}
              pageSize={8}
              onChange={handlePageChange}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default AuctionHistory;
