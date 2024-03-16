import { MagnifyingGlassIcon } from "@heroicons/react/20/solid";
import { Input } from "@material-tailwind/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import {
  Table,
  TableProps,
  Tag,
  Descriptions,
  Button,
  notification,
} from "antd";
import { useState, useEffect } from "react";
import {
  getStaff,
  searchStaff,
  getAccountStaffId,
  changeStatusStaff,
} from "../../../../api/staff";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";

const AdminStaffList: React.FC = () => {
  const [search, setSearch] = useState<searchStaff>({ KeyWord: "" });
  const [staffData, setStaffData] = useState<Staff[]>(); // State để lưu trữ dữ liệu nhân viên
  const [staffDetailData, setStaffDetailData] = useState<staffDetail>();
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const [accountStaffId, setAccountId] = useState<Number>();
  const [statusStaff, setStatusAccount] = useState<Number>();
  const { token } = useContext(UserContext);

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

  const fetchChangeStatus = async (
    Id: Number | undefined,
    status: Number | undefined
  ) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await changeStatusStaff(Id, status, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching change status:", error);
    }
  };

  const fetchStaffDetail = async (accountId: Number | undefined) => {
    try {
      if (token) {
        let data: staffDetail | undefined;
        data = await getAccountStaffId(accountId, token);
        setStaffDetailData(data);
        setAccountId(accountId);
        if (data?.account_Status == "Active") {
          setStatusAccount(0);
        } else {
          setStatusAccount(1);
        }
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching member detail:", error);
    }
  };

  const fetchStaffList = async () => {
    try {
      if (token) {
        let data: Staff[] | undefined;
        if (search.KeyWord !== "") {
          data = await searchStaff(search, token);
        } else {
          data = await getStaff(token);
        }
        setStaffData(data);
      }
    } catch (error) {
      console.error("Error fetching member list:", error);
    }
  };

  useEffect(() => {
    fetchStaffList();
  }, [search, token]);

  const viewDetail = (AccountId: Number) => {
    fetchStaffDetail(AccountId);
  };

  const columns: TableProps<Staff>["columns"] = [
    {
      title: "Account Name",
      dataIndex: "accountName",
      width: "20%",
    },
    {
      title: "Username",
      dataIndex: "username",
      width: "10%",
    },
    {
      title: "Account Email",
      dataIndex: "accountEmail",
      width: "20%",
    },
    {
      title: "Status",
      dataIndex: "account_Status",
      width: "5%",
      render: (account_Status: string) => {
        let color = account_Status === "Block" ? "volcano" : "green";
        return (
          <Tag color={color} key={account_Status}>
            {account_Status.toUpperCase()}
          </Tag>
        );
      },
    },
    {
      title: "Date Created",
      dataIndex: "date_Created",
      width: "18%",
      render: (date_Created: Date) => formatDate(date_Created),
    },
    {
      title: "Date End",
      dataIndex: "date_End",
      width: "18%",
      render: (date_End: Date) => formatDate(date_End),
    },

    {
      title: "Actions",
      dataIndex: "operation",
      render: (_: any, staff: Staff) => (
        <a onClick={() => viewDetail(staff.accountId)}>View details</a>
      ),
      width: "15%",
    },
  ];

  const renderBorderedItems = () => {
    const items = [
      {
        key: "1",
        label: "Account Name",
        children: staffDetailData?.accountName || "",
      },
      {
        key: "2",
        label: "Username",
        children: staffDetailData?.username || "",
      },
      {
        key: "3",
        label: "Account Email",
        children: staffDetailData?.accountEmail || "",
      },
      {
        key: "4",
        label: "Phone Number",
        children: staffDetailData?.phoneNumber || "",
      },
      {
        key: "5",
        label: "Citizen Identification",
        children: staffDetailData?.citizen_identification || "",
      },
      {
        key: "6",
        label: "Address",
        children: staffDetailData?.address || "",
      },
      {
        key: "7",
        label: "Account Status",
        children: staffDetailData?.account_Status || "",
        render: (account_Status: string) => {
          let color = account_Status === "Block" ? "volcano" : "green";
          return (
            <Tag color={color} key={account_Status}>
              {account_Status.toUpperCase()}
            </Tag>
          );
        },
      },
      {
        key: "8",
        label: "Date Created",
        children: staffDetailData
          ? formatDate(staffDetailData.date_Created)
          : "",
      },
      {
        key: "9",
        label: "Date End",
        children: staffDetailData ? formatDate(staffDetailData.date_End) : "",
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
    fetchStaffList(); // Gọi lại hàm fetchMemberList khi quay lại danh sách
  };
  const getMessage = async () => {
    const response = await fetchChangeStatus(accountStaffId, statusStaff);
    if (response !== undefined && response) {
      // Kiểm tra xem response có được trả về hay không
      if (response.statusCode === "MSG17") {
        openNotificationWithIcon("success", response.message);
      } else {
        openNotificationWithIcon("error", "Something went wrong when executing operation. Please try again!");
      }
      await fetchStaffDetail(accountStaffId);
    }
  };
  
  const openNotificationWithIcon = (type: 'success' | 'error', description: string) => {
    notification[type]({
      message: "Notification Title",
      description: description,
    });
  };

  return (
    <>
      {showDetail ? (
        <div>
          <Button onClick={handleBackToList}>
            <FontAwesomeIcon icon={faArrowLeft} style={{ color: "#74C0FC" }} />
          </Button>
          <br />
          <br />
          <div style={{ display: "flex", justifyContent: "flex-end" }}>
            <Button onClick={getMessage}>Change Status</Button>
          </div>
          <br />
          <Descriptions bordered title="Detail of Staff">
            {renderBorderedItems()}
          </Descriptions>
        </div>
      ) : (
        <div>
          {/* Bảng danh sách */}
          <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
            <div className="w-full md:w-72 flex flex-row justify-start">
              <Input
                label="Search by Name or Email"
                icon={<MagnifyingGlassIcon className="h-5 w-5" />}
                crossOrigin={undefined}
                onKeyDown={(e) => {
                  if (e.key === "Enter") {
                    setSearch({ KeyWord: e.currentTarget.value });
                  }
                }}
              />
            </div>
          </div>
          <Table columns={columns} dataSource={staffData} bordered />
        </div>
      )}
    </>
  );
};

export default AdminStaffList;
