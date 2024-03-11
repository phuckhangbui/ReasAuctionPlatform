import { MagnifyingGlassIcon } from "@heroicons/react/20/solid";
import { Input } from "@material-tailwind/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import {
  Table,
  TableProps,
  Tag,
  Button,
  Modal,
  DatePicker,
  notification
} from "antd";
import {NumberFormat} from "../../../../Utils/numberFormat";
import { useState, useEffect } from "react";
import {
  getRealForDeposit,
  getUserForDeposit,
  addAuction
} from "../../../../api/adminAuction";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";

const RealDepositList: React.FC = () => {
  const { token } = useContext(UserContext);
  const [search, setSearch] = useState("");
  const [RealData, setRealData] = useState<RealForDeposit[]>();
  const [DepositData, setDepoistData] = useState<DepositAmountUser[]>();
  const [reasID, setRealID] = useState<number | undefined>();
  const [showDetail, setShowDetail] = useState<boolean>(false);
  let dateStart: Date = new Date();


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
    Deposited: "blue",
    Waiting_for_refund: "yellow",
    Refunded: "gray",
  };

  const fetchRealList = async () => {
    try {
      if (token) {
        let data: RealForDeposit[] | undefined;
        data = await getRealForDeposit(token);
        setRealData(data);
      }
    } catch (error) {
      console.error("Error fetching real list:", error);
    }
  };

  useEffect(() => {
    fetchRealList();
  }, [token]);

  const fetchDepositUser = async (reasId: number) => {
    try {
      if (token) {
        let data: DepositAmountUser[] | undefined;
        data = await getUserForDeposit(token, reasId);
        setDepoistData(data);
        setRealID(reasId);
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching deposit detail:", error);
    }
  };

  const viewDetail = (reasID: number) => {
    fetchDepositUser(reasID);
  };

  const fetchCreateAuction = async (Auction: AuctionCreate) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await addAuction(Auction, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching add news:", error);
    }
  };

  const columns: TableProps<RealForDeposit>["columns"] = [
    {
      title: "No",
      width: "5%",
      render: (text: any, record: any, index: number) => index + 1,
    },
    {
      title: "Reas Name",
      dataIndex: "reasName",
      width: "30%",
    },
    {
      title: "Number of user",
      dataIndex: "numberOfUser",
      width: "15%",
      render: (num: number) => `${num} users`,
    },
    {
      title: "",
      dataIndex: "operation",
      render: (_: any, record: RealForDeposit) => (
        <a onClick={() => viewDetail(record.reasId)}>View details</a>
      ),
      width: "10%",
    },
  ];

  const [isModalOpen, setIsModalOpen] = useState(false);
  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleOk = () => {
    setIsModalOpen(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const onChangeDate = (date: any, dateString: any) => {
    dateStart = date;
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

  const createAuction = async () => {
    const Auction : AuctionCreate = {
AccountCreateId : 1,
DateStart : dateStart,
ReasId : reasID,
    }
    const response = await fetchCreateAuction(Auction);
    if (response != undefined && response) {
      if (response.statusCode == "MSG05") {
        openNotificationWithIcon("success", response.message);
      } else {
        openNotificationWithIcon(
          "error",
          "Something went wrong when executing operation. Please try again!"
        );
      }
    }
  };

  const columnUsers: TableProps<DepositAmountUser>["columns"] = [
    {
      title: "No",
      width: "5%",
      render: (text: any, record: any, index: number) => index + 1,
    },
    {
      title: "Account Name",
      dataIndex: "accountName",
      width: "15%",
    },
    {
      title: "Account Email",
      dataIndex: "accountEmail",
      width: "15%",
    },
    {
      title: "Account Phone",
      dataIndex: "accountPhone",
      width: "10%",
    },
    {
      title: "Deposit Amount",
      dataIndex: "amount",
      width: "10%",
      render: (depositAmount: number) => NumberFormat(depositAmount)
    },
    {
      title: "Deposit Date",
      dataIndex: "depositDate",
      render: (depositDate: Date) => formatDate(depositDate),
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
  ];

  const handleBackToList = () => {
    setShowDetail(false); // Ẩn bảng chi tiết và hiện lại danh sách
    fetchRealList(); // Gọi lại hàm fetchMemberList khi quay lại danh sách
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
          <div>
            <Button onClick={showModal}>Create Auction</Button>
            <Modal
              title="Fill information to create Auction"
              open={isModalOpen}
              onOk={handleOk}
              onCancel={handleCancel}
            >
              <div style={{alignContent: "center"}}>
              <DatePicker
                onChange={onChangeDate}
                showTime
                needConfirm={false}
              />
              </div>
              <br />
            </Modal>
          </div>
          <br />
          <Table columns={columnUsers} dataSource={DepositData} bordered />
        </div>
      ) : (
        <div>
          <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
            <div className="w-full md:w-72 flex flex-row justify-start"></div>

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
            dataSource={RealData?.filter((reas: RealForDeposit) => {
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

export default RealDepositList;
