import { MagnifyingGlassIcon } from "@heroicons/react/20/solid";
import { Input } from "@material-tailwind/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { NumberFormat } from "../../../../Utils/numberFormat";
import {
  Table,
  TableProps,
  Tag,
  Button,
  Descriptions,
  Modal,
  notification,
  Input as InputAntd,
} from "antd";
import { useState, useEffect, useContext } from "react";
import {
  getAuctionCompleteAdmin,
  getAuctionCompleteAdminById,
  getPaticipateUser,
  changeMemberWin,
  changeSuccessReal
} from "../../../../api/adminAuction";
import { UserContext } from "../../../../context/userContext";

const CompleteList: React.FC = () => {
  const { token } = useContext(UserContext);
  const [search, setSearch] = useState("");
  const [auctionData, setAuctionData] = useState<AuctionAdmin[]>();
  const [auctionDetailData, setAuctionDetailData] =
    useState<AuctionDetailCompleteAdmin>();
  const [participateData, setParticipateData] =
    useState<ParticipateAccount[]>();
  const [auctionID, setAuctionID] = useState<number | undefined>();
  const [reasID, setReasID] = useState<number | undefined>();
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
        data = await getAuctionCompleteAdmin(token);
        setAuctionData(data);
      }
    } catch (error) {
      console.error("Error fetching auction list:", error);
    }
  };

  useEffect(() => {
    fetchAuctionList();
  }, [token]);

  const fetchAuctionDetail = async (auctionId: number | undefined) => {
    try {
      if (token) {
        let data: AuctionDetailCompleteAdmin | undefined;
        data = await getAuctionCompleteAdminById(auctionId, token);
        setAuctionDetailData(data);
        setAuctionID(auctionId);
        setReasID(data?.reasId);
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching auction detail:", error);
    }
  };

  const fetchParticipate = async (auctionId: number | undefined) => {
    try {
      if (token) {
        let data: ParticipateAccount[] | undefined;
        data = await getPaticipateUser(token, auctionId);
        setParticipateData(data);
        setAuctionID(auctionId);
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching participate users:", error);
    }
  };

  const fetchChangeMemberWin = async (dataReponse: AuctionChangeMember) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await changeMemberWin(dataReponse, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching change member:", error);
    }
  };

  const fetchChangeContactSuccess= async (reasId: number | undefined) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await changeSuccessReal(reasId, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching change contact success:", error);
    }
  };

  const viewDetail = (AuctionId: number) => {
    fetchAuctionDetail(AuctionId);
    fetchParticipate(AuctionId);
  };

  const columns: TableProps<AuctionAdmin>["columns"] = [
    {
      title: "No",
      width: "5%",
      render: (_text: any, _record: any, index: number) => index + 1,
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
      width: "10%",
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
        label: "Account Winner",
        children: auctionDetailData?.accountWinnerName || "",
      },
      {
        key: "7",
        label: "Email Winner",
        children: auctionDetailData?.accountWinnerEmail || "",
      },
      {
        key: "8",
        label: "Phone Winner",
        children: auctionDetailData?.accountWinnerPhone || "",
      },
      {
        key: "9",
        label: "Foor Bid",
        children: auctionDetailData?.floorBid
          ? NumberFormat(auctionDetailData?.floorBid)
          : "",
      },
      {
        key: "10",
        label: "Final Amount",
        children: auctionDetailData?.finalAmount
          ? NumberFormat(auctionDetailData?.floorBid)
          : "",
      },
      {
        key: "11",
        label: "Deposit Amount",
        children: auctionDetailData?.depositAmout
          ? NumberFormat(auctionDetailData?.depositAmout)
          : "",
      },
      {
        key: "12",
        label: "Commision Amount",
        children: auctionDetailData?.commisionAmount
          ? NumberFormat(auctionDetailData?.commisionAmount)
          : "",
      },
      {
        key: "13",
        label: "Recieve Amount",
        children: auctionDetailData?.ownerReceiveAmount
          ? NumberFormat(auctionDetailData?.ownerReceiveAmount)
          : "",
      },
      {
        key: "14",
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
        key: "15",
        label: "Date Start",
        children: auctionDetailData
          ? formatDate(auctionDetailData.dateStart)
          : "",
      },
      {
        key: "16",
        label: "Date End",
        children: auctionDetailData
          ? formatDate(auctionDetailData.dateEnd)
          : "",
      },
    ];
    return items.map((item) => (
      <Descriptions.Item key={item.key} label={item.label}>
        {item.render ? item.render(item.children) : item.children}
      </Descriptions.Item>
    ));
  };

  const columnParticipate: TableProps<ParticipateAccount>["columns"] = [
    {
      title: "No",
      width: "5%",
      render: (_text: any, _record: any, index: number) => index + 1,
    },
    {
      title: "Account Name",
      dataIndex: "accountName",
      width: "20%",
    },
    {
      title: "Account Email",
      dataIndex: "accountEmail",
      width: "15%",
    },
    {
      title: "Account Phone",
      dataIndex: "accountPhone",
      width: "15%",
    },
    {
      title: "Last Bid",
      dataIndex: "lastBid",
      render: (lastBid: number) => NumberFormat(lastBid),
      width: "15%",
    },
    {
      title: "Note",
      dataIndex: "note",
      width: "20%",
    },
  ];

  const handleBackToList = () => {
    setShowDetail(false); // Ẩn bảng chi tiết và hiện lại danh sách
    fetchAuctionList(); // Gọi lại hàm fetchMemberList khi quay lại danh sách
  };

  const [isModalOpen, setIsModalOpen] = useState(false);
  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleOk = () => {
    setIsModalOpen(false);
    handleChangeMember();
    setShowDetail(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  let noteReason: string = "";
  const onChangeMember = (note: any) => {
    noteReason = note;
  };

  const openNotificationWithIcon = (
    type: "success" | "error",
    description: string
  ) => {
    notification[type]({
      message: "Notification Title",
      description: description,
    });
  };

  const handleChangeMember = async () => {
    const Auction: AuctionChangeMember = {
      auctionId: auctionID,
      note: noteReason,
    };
    const response = await fetchChangeMemberWin(Auction);
    if (response != undefined && response) {
      if (response.statusCode == "MSG27") {
        openNotificationWithIcon("success", response.message);
        fetchAuctionDetail(auctionID);
        fetchParticipate(auctionID);
      } 
      else if (response.statusCode == "MSG28") {
        openNotificationWithIcon("success", response.message);
        fetchAuctionDetail(auctionID);
        fetchParticipate(auctionID);
      }
      else {
        openNotificationWithIcon(
          "error",
          "Something went wrong when executing operation. Please try again!"
        );
      }
    }
  };

  const handleChangeContactSuccess = async () => {
    const response = await fetchChangeContactSuccess(reasID);
    if (response != undefined && response) {
      if (response.statusCode == "MSG30") {
        openNotificationWithIcon("success", response.message);
        fetchAuctionDetail(auctionID);
        fetchParticipate(auctionID);
      } 
      else {
        openNotificationWithIcon(
          "error",
          "Something went wrong when executing operation. Please try again!"
        );
      }
    }
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
          <div style={{ display: "flex", justifyContent: "flex-end" }}>
          <Button onClick={handleChangeContactSuccess} style={{backgroundColor: "green"}}>Contact Success</Button>
            <Button onClick={showModal}>Change Member Win</Button>
            <Modal
              title="Fill information to create Auction"
              open={isModalOpen}
              onOk={handleOk}
              onCancel={handleCancel}
              footer={[
                <Button key="submit" onClick={handleOk}>
                  Create
                </Button>,
              ]}
            >
              <div style={{ alignContent: "center" }}>
                <p>
                  <strong>Note :</strong>
                </p>
                <InputAntd
                  placeholder="Note reason to change winner"
                  onChange={onChangeMember}
                />
              </div>
              <br />
            </Modal>
          </div>
          <br />
          <br />
          <Descriptions bordered title="Detail of Auction">
            {renderBorderedItems()}
          </Descriptions>

          <br />
          <br />
          <h4>
            <strong>Participate List</strong>
          </h4>
          <Table
            columns={columnParticipate}
            dataSource={participateData}
            bordered
          />
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
            dataSource={auctionData?.filter((reas: AuctionAdmin) => {
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
};

export default CompleteList;

// const CompleteList: React.FC = () => {return <></>}
// export default CompleteList;
