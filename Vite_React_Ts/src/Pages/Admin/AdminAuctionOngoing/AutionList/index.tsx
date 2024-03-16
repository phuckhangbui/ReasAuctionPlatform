import { MagnifyingGlassIcon } from "@heroicons/react/20/solid";
import {Input} from "@material-tailwind/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { Table, TableProps, Tag , Button, Descriptions} from "antd";
import { useState, useEffect } from "react";
import {
  getAuctionAllAdmin,
  getAuctionAllAdminById,
} from "../../../../api/adminAuction";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";
import { NumberFormat } from "../../../../Utils/numberFormat";

const AuctionAllList: React.FC = () => {
  const { token } = useContext(UserContext);
  const [search, setSearch] = useState("");
  const [auctionData, setAuctionData] = useState<AuctionAdmin[]>();
  const [auctionDetailData, setAuctionDetailData] = useState<AuctionDetailAllAdmin>();
  const [auctionID, setAuctionID] = useState<number>();
  const [showDetail, setShowDetail] = useState<boolean>(false);

  const formatDate = (dateString: Date): string => {
    const dateObject = new Date(dateString);
    return `${dateObject.getFullYear()}-${(
      "0" +
      (dateObject.getMonth() + 1)
    ).slice(-2)}-${("0" + dateObject.getDate()).slice(-2)} ${(
      "0" + dateObject.getHours()
    ).slice(-2)}:${("0" + dateObject.getMinutes()).slice(-2)}:${(
      "0" + dateObject.getSeconds()
    ).slice(-2)}`;
  };

  const statusColorMap: { [key: string]: string } = {
    NotYet: "gray",
    OnGoing: "yellow",
    Finish: "blue",
    Cancel: "volcano",
  };

  const fetchAuctionList = async () => {
    try {
      if (token) {
        let data: AuctionAdmin[] | undefined;
        data = await getAuctionAllAdmin(token);
        setAuctionData(data);
      }
    } catch (error) {
      console.error("Error fetching auction list:", error);
    }
  };

  useEffect(() => {
    fetchAuctionList();
  }, [token]);

  const fetchAuctionDetail = async (auctionId: Number | undefined) => {
    try {
      if (token) {
        let data: AuctionDetailAllAdmin | undefined;
        data = await getAuctionAllAdminById(auctionId, token);
        setAuctionDetailData(data);
        setAuctionID(auctionID);
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching auction detail:", error);
    }
  };

  const viewDetail = (AuctionId: number) => {
    fetchAuctionDetail(AuctionId);
  };

  const columns: TableProps<AuctionAdmin>["columns"] = [
    {
      title: "No",
      width: "5%",
      render: (index: number) => index + 1,
    },
    {
      title: "Reas Name",
      dataIndex: "reasName",
      width: "20%",
    },
    {
      title: "Floor Bid",
      dataIndex: "floorBid",
      render: (floorBid: number) => NumberFormat(floorBid),
      width: "15%",
    },
    {
      title: "Date Start",
      dataIndex: "dateStart",
      render: (date_Created: Date) => formatDate(date_Created),
      width: "15%",
    },
    {
      title: "Date End",
      dataIndex: "dateEnd",
      render: (date_End: Date) => formatDate(date_End),
      width: "15%",
    },
    {
      title: "Status",
      dataIndex: "status",
      width: "10%",
      render: (reas_Status: string) => {
        const color = statusColorMap[reas_Status] || "gray"; // Mặc định là màu xám nếu không có trong ánh xạ
        return (
          <Tag color={color} key={reas_Status}>
            {reas_Status.toUpperCase()}
          </Tag>
        );
      },
    },
    {
      title: "",
      dataIndex: "operation",
      render: (_: any, record: AuctionAdmin) => (
        <a onClick={() => viewDetail(record.auctionId)}>View details</a>
      ),
      width: "10%",
    },
  ];

  const renderBorderedItems = () => {
    const items = [
      {
        key: "1",
        label: "Reas Name",
        children: auctionDetailData?.reasName || "",
        span: 3,
      },
      {
        key: "2",
        label: "Account Create",
        children: auctionDetailData?.accountCreateName || "",
      },
      {
        key: "3",
        label: "Account Owner",
        children: auctionDetailData?.accountOwnerName || "",
      },
      {
        key: "4",
        label: "Email Owner",
        children: auctionDetailData?.accountOwnerEmail || "",
      },
      {
        key: "5",
        label: "Phone Owner",
        children: auctionDetailData?.accountOwnerPhone || "",
      },
      {
        key: "6",
        label: "Foor Bid",
        children: auctionDetailData?.floorBid ? NumberFormat(auctionDetailData?.floorBid) : "",
      },
      {
        key: "7",
        label: "Account Status",
        children: auctionDetailData?.status || "",
        render: (reas_Status: string) => {
          const color = statusColorMap[reas_Status] || "gray"; // Mặc định là màu xám nếu không có trong ánh xạ
          return (
            <Tag color={color} key={reas_Status}>
              {reas_Status.toUpperCase()}
            </Tag>
          );
        },
      },
      {
        key: "8",
        label: "Date Start",
        children: auctionDetailData
          ? formatDate(auctionDetailData.dateStart)
          : "",
      },
    ];
    return items.map((item) => (
      <Descriptions.Item key={item.key} label={item.label}>
        {item.render ? item.render(item.children) : item.children}
      </Descriptions.Item>
    ));
  };


  const handleBackToList = () => {
    setShowDetail(false); // Ẩn bảng chi tiết và hiện lại danh sách
    fetchAuctionList(); // Gọi lại hàm fetchMemberList khi quay lại danh sách
  };


  // Generate random dates within a range of 10 years from today

  // Generate 100 random CompletedAuction items


  return (
    <>
     {showDetail ? (
        <div>
          <Button onClick={handleBackToList}>
            <FontAwesomeIcon icon={faArrowLeft} style={{ color: "#74C0FC" }} />
          </Button>
          <br />
          <br />
          <Descriptions bordered title="Detail of Auction">
            {renderBorderedItems()}
          </Descriptions>
        </div>
      ) : (
        <div>
      <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
        <div className="w-full md:w-72 flex flex-row justify-start">
          {/* <Select
            defaultValue="Residential"
            onChange={handleChange}
            options={[
              { value: "residential", label: "Residential" },
              { value: "commercial", label: "Commercial" },
              { value: "land", label: "Land" },
              { value: "investment", label: "Investment" },
              { value: "farmland", label: "Farmland" },
              { value: "specialPurpose", label: "Special Purpose" },
              { value: "department", label: "Department" },
            ]}
          /> */}
        </div>

        <div className="w-full md:w-72 flex flex-row justify-end">
          <div className="flex flex-row space-between space-x-2">
            <Input
              label="Search"
              icon={<MagnifyingGlassIcon className="h-5 w-5" />}
              crossOrigin={undefined}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>
        </div>
      </div>
      <Table
        columns={columns}
        dataSource={auctionData?.filter((reas : AuctionAdmin) => {
          const isMatchingSearch =
            search.toLowerCase() === "" ||
            reas.reasName.toLowerCase().includes(search);

          return isMatchingSearch;
        })}
        bordered
      />
      </div>
      )}
    </>
  );
}

export default AuctionAllList;
