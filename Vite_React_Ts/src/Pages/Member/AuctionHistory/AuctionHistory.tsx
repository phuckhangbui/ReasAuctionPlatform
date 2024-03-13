import {
  Card,
  CardHeader,
  CardBody,
  CardFooter,
  Typography,
  Chip,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { NumberFormat } from "../../../utils/numbetFormat";
import { DatePicker, Pagination, Select } from "antd";
import { useLocation } from "react-router-dom";



const AuctionHistory: React.FC = () => {
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [displayedAuctions, setDisplayedAuctions] = useState<
    AuctionAccounting[]
  >([]);
  const [auctionAccounting, setAuctionAccounting] = useState<
    AuctionAccounting[] | undefined
  >([]);

  useEffect(() => {
    const fetchedAuctions: AuctionAccounting[] = [];
    for (let i = 1; i <= 10; i++) {
      fetchedAuctions.push({
        AuctionAccountingId: i,
        AuctionId: i,
        ReasId: i,
        ReasName: `Reas No. ${i}`,
        AccountWinId: 10 - i,
        uriPhotoFirst:
          "https://res.cloudinary.com/dqpsvl3nu/image/upload/v1708523932/3_CH%E1%BB%A6%20C%E1%BA%A6N%20B%C3%81N%20L%C3%94%20%C4%90%E1%BA%A4T%20%C3%94M%20SU%E1%BB%90I%20L%C3%8AN%20%C4%90%C6%AF%E1%BB%A2C%20TH%E1%BB%94%20C%C6%AF_L%C3%A2m%20Nh%E1%BA%ADt%20Anh/nv7oorerx0nxqb7a4afg.jpg",
        MaxAmount: 100000 + i,
        AuctionType: "Department",
        Area: 100,
        EstimatedPaymentDate: new Date(),
        Status: 1,
      });
    }
    setAuctionAccounting(fetchedAuctions);
  }, []);

  

  // Update displayed auctions when currentPage or auctionAccounting changes
  useEffect(() => {
    // Calculate the start index and end index of auctions to display based on currentPage
    const pageSize = 8; // Change this value according to your requirement
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(
      startIndex + pageSize,
      auctionAccounting?.length || 0
    );

    // Extract the auctions to be displayed for the current page
    const auctionsToDisplay =
      auctionAccounting?.slice(startIndex, endIndex) || [];

    setDisplayedAuctions(auctionsToDisplay);
  }, [currentPage, auctionAccounting]);

  // Function to handle page change
  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const options = [
    {
      value: "department",
      label: "Department",
    },
  ];
  const handleChange = (value: string) => {
    console.log(`selected ${value}`);
  };
  const { RangePicker } = DatePicker;
  const optionsOrderBy = [
    {
      value: "name_acs",
      label: "Name from A-Z",
    },
    {
      value: "decs",
      label: "Name from Z-A",
    },
    {
      value: "price_asc",
      label: "Price gradually increase",
    },
    {
      value: "price_decs",
      label: "Price gradually decrease",
    },
  ];
  return (
    <>
      <div className="pt-20">
        <div className="container w-full mx-auto">
          <div className="flex justify-between">
            <div className="w-1/2">
              <Select
                style={{ width: "30%" }}
                placeholder="Type"
                onChange={handleChange}
                options={options}
                defaultValue={options[0].label}
              />
              <RangePicker />
            </div>
            <div className="w-1/2 flex justify-end">
              <Select
                style={{ width: "30%" }}
                placeholder="Order by"
                onChange={handleChange}
                options={optionsOrderBy}
                defaultValue={optionsOrderBy[0].label}
              />
            </div>
          </div>
        </div>
      </div>
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <div className="mt-4 grid lg:grid-cols-4 md:grid-cols-2 md:gap-3 sm:grid-cols-1">
            {displayedAuctions &&
              displayedAuctions.map((auction) => (
                <Card className="max-w-[24rem] overflow-hidden">
                  <CardHeader
                    floated={false}
                    shadow={false}
                    color="transparent"
                    className="m-0 rounded-none"
                  >
                    <img src={auction.uriPhotoFirst} alt="ui/ux review check" />
                  </CardHeader>
                  <CardBody>
                    <Typography variant="h4" color="blue-gray">
                      {auction.ReasName}
                    </Typography>
                    <Typography className="font-normal">
                      {auction.EstimatedPaymentDate.toDateString()}
                    </Typography>
                    <div className="mb-3 font-normal text-gray-700">
                      <span className="text-gray-900 font-semibold">
                        {auction.AuctionType}
                      </span>
                      <span className="sm:inline md:hidden xl:inline"> | </span>
                      <br className="sm:hidden md:block xl:hidden" />
                      <span className="text-gray-900 font-semibold">
                        {auction.Area}
                      </span>
                      <span> sqrt</span>
                    </div>
                  </CardBody>
                  <CardFooter className="flex items-center justify-between">
                    <div className="flex items-center -space-x-3">
                      <Typography variant="h6" color="blue-gray">
                        {NumberFormat(auction.MaxAmount)}
                      </Typography>
                    </div>
                    <Chip color="blue" value="finish" />
                  </CardFooter>
                </Card>
              ))}
          </div>
          <div className="flex justify-center mt-4">
            <Pagination
              defaultCurrent={1}
              total={auctionAccounting?.length || 0}
              pageSize={8} // Change this value according to your requirement
              onChange={handlePageChange}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default AuctionHistory;
