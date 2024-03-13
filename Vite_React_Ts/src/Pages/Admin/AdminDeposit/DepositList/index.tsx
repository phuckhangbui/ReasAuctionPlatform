import React, { useCallback, useContext, useEffect, useState } from "react";
import { UserContext } from "../../../../context/userContext";
import { Button, Input, Table, TableProps, Tag } from "antd";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { getDeposit, getReasDeposited } from "../../../../api/deposit";

const statusStringMap: { [key: number]: string } = {
  0: "Deposited",
  1: "Waiting",
  2: "Refunded",
};
const statusUserStringMap: { [key: number]: string } = {
  0: "Block",
  1: "Active",
};

const statusUserColorMap: { [key: string]: string } = {
  Active: "green",
  Block: "volcano",
};

const statusDepositColorMap: { [key: string]: string } = {
  Deposited: "green",
  Waiting: "orange",
  Refunded: "blue",
};

const AllDepositsList: React.FC = () => {
  const [search, _setSearch] = useState<searchDeposit>({
    reasName: "",
    PageNumber: 1,
    PageSize: 15,
  });
  const [depositsList, setDepositsList] = useState<deposit[]>(); // State để lưu trữ dữ liệu nhân viên
  const [depositDetail, setDepositDetail] = useState<depositDetail[]>([]);
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const { token } = useContext(UserContext);

  useEffect(() => {
    try {
      const fetchData = async () => {
        if (token) {
          if (search) {
            const reponse = await getDeposit(token, search.reasName);
            setDepositsList(reponse);
          }
        }
      };
      fetchData();
    } catch (error) {
      console.log(error);
    }
  }, []);

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
  const viewDepositDetail = (reasId: number) => {
    try {
      const fetchDepositDetail = async () => {
        if (token && reasId) {
          const response = await getReasDeposited(token, reasId);
          if (response) {
            setDepositDetail(response);
          }
          setShowDetail(true);
        }
      };
      fetchDepositDetail();
    } catch (error) {
      console.log(error);
    }
  };

  const FormSearch = () => {
    const [searchValue, setSearchValue] = useState("");
    const handleSearch = useCallback(async () => {
      try {
        if (token) {
          const response = await getDeposit(token, searchValue);
          setDepositsList(response);
        }
      } catch (error) {
        console.log(error);
      }
    }, [searchValue, token]);
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      setSearchValue(e.target.value);
    };

    return (
      <div>
        <Input
          placeholder="Name"
          value={searchValue || ""}
          onChange={handleChange}
        />
        <Button onClick={handleSearch}>Search</Button>
      </div>
    );
  };

  const columns: TableProps<deposit>["columns"] = [
    {
      title: "Real-Estate Name",
      dataIndex: "reasName",
      width: "20%",
    },
    {
      title: "Account Signed",
      dataIndex: "accountSignName",
      width: "20%",
    },
    {
      title: "Money",
      dataIndex: "amount",
      width: "20%",
    },
    {
      title: "Deposit Date",
      dataIndex: "depositDate",
      width: "15%",
      render: (date_Deposit: Date) => formatDate(date_Deposit),
    },
    {
      title: "Created Date",
      dataIndex: "createDepositDate",
      width: "15%",
      render: (date_Created: Date) => formatDate(date_Created),
    },
    {
      title: "Status",
      dataIndex: "status",
      width: "5%",
      render: (status: number) => {
        if (status !== undefined) {
          const color = statusDepositColorMap[statusStringMap[status]];

          return (
            <Tag color={color} key={statusStringMap[status]}>
              {statusStringMap[status]}
            </Tag>
          );
        } else {
          return null;
        }
      },
    },

    {
      title: "Actions",
      dataIndex: "operation",
      render: (_: any, deposit: deposit) => (
        <a onClick={() => viewDepositDetail(deposit.reasId)}>View details</a>
      ),
      width: "15%",
    },
  ];

  const detailColumns: TableProps<depositDetail>["columns"] = [
    {
      title: "Account",
      dataIndex: "accountName",
      width: "20%",
    },
    {
      title: "Email",
      dataIndex: "accountEmail",
      width: "20%",
    },
    {
      title: "Status",
      dataIndex: "account_Status",
      width: "5%",
      render: (status: number) => {
        if (status !== undefined) {
          const color = statusUserColorMap[statusUserStringMap[status]];

          return (
            <Tag color={color} key={statusUserStringMap[status]}>
              {statusUserStringMap[status]}
            </Tag>
          );
        } else {
          return null;
        }
      },
    },
    {
      title: "Phone Number",
      dataIndex: "phoneNumber",
      width: "20%",
    },
    {
      title: "Created Date",
      dataIndex: "date_Created",
      width: "15%",
      render: (date_Created: Date) => formatDate(date_Created),
    },
  ];

  const handleBackToList = () => {
    setShowDetail(false);
  };

  return (
    <>
      {showDetail ? (
        <div>
          <Button onClick={handleBackToList}>
            <FontAwesomeIcon icon={faArrowLeft} style={{ color: "#74C0FC" }} />
          </Button>
          <br />
          <div>
            <div>
              <h4 className="font-semibold py-5">Real Estate's Depositors</h4>
              <Table
                columns={detailColumns}
                dataSource={depositDetail}
                bordered
              />
              <br /> <br />
            </div>
          </div>
        </div>
      ) : (
        <div>
          {/* Bảng danh sách */}
          <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
            <div className="w-full md:w-72 flex flex-row justify-start">
              <FormSearch />
            </div>
          </div>
          <Table columns={columns} dataSource={depositsList} bordered />
        </div>
      )}
    </>
  );
};

export default AllDepositsList;
