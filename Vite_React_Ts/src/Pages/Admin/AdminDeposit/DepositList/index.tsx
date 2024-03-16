import React, { useCallback, useContext, useEffect, useState } from "react";
import { UserContext } from "../../../../context/userContext";
import { Button, Table, TableProps, Tag, notification } from "antd";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import {
  getDeposit,
  getReasDeposited,
  getReasName,
} from "../../../../api/deposit";
import { CreateTransactionRefund } from "../../../../api/transaction";
import { MagnifyingGlassIcon } from "@heroicons/react/20/solid";
import { Input } from "@material-tailwind/react";
import { NumberFormat } from "../../../../Utils/numberFormat";

const statusStringMap: { [key: number]: string } = {
  2: "Selling",
  4: "Auctioning",
  5: "Sold",
};

const statusDepositColorMap: { [key: string]: string } = {
  Selling: "yellow",
  Auctioning: "green",
  Sold: "orange",
};

const statusDepositUserColorMap: { [key: string]: string } = {
  Pending: "yellow",
  Deposited: "green",
  Winner: "green",
  Waiting_for_refund: "orange",
  Refunded: "red",
};

const AllDepositsList: React.FC = () => {
  const [depositsList, setDepositsList] = useState<deposit[]>(); // State để lưu trữ dữ liệu nhân viên
  const [depositDetail, setDepositDetail] = useState<DepositAmountUser[]>([]);
  const [reasName, setReasName] = useState<string>();
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const [search, setSearch] = useState("");
  const { token } = useContext(UserContext);

  useEffect(() => {
    try {
      const fetchData = async () => {
        if (token) {
          const reponse = await getDeposit(token);
          setDepositsList(reponse);
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

  const fetchUpdateStatus = async (newTransaction: TransactionCreateRefund) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await CreateTransactionRefund(token, newTransaction);
        return data;
      }
    } catch (error) {
      console.error("Error fetching add transaction:", error);
    }
  };

  const viewReasName = (reasId: number) => {
    try {
      const fetchReasName = async () => {
        if (token && reasId) {
          const response = await getReasName(token, reasId);
          if (response) {
            setReasName(response);
          }
          setShowDetail(true);
        }
      };
      fetchReasName();
    } catch (error) {
      console.log(error);
    }
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

  const handleChangeStatus = async (
    accountid: number,
    reasID: number,
    depositAmount: number,
    depositID: number
  ) => {
    try {
      const transaction: TransactionCreateRefund = {
        accountReceiveId: accountid,
        depositId: depositID,
        money: depositAmount,
        reasId: reasID,
      };
      const response = await fetchUpdateStatus(transaction);
      if (response !== undefined) {
        // Kiểm tra xem response có được trả về hay không
        if (response.statusCode === "MSG26") {
          openNotificationWithIcon("success", response.message);
        } else {
          openNotificationWithIcon(
            "error",
            "Something went wrong when executing operation. Please try again!"
          );
        }
      }
      setDetail(transaction.reasId);
    } catch (error) {
      console.error("Error handling status change:", error);
    }
  };

  const setDetail = (id: number) => {
    viewDepositDetail(id);
    viewReasName(id);
  };

  const columns: TableProps<deposit>["columns"] = [
    {
      title: "Real-Estate Name",
      dataIndex: "reasName",
      width: "20%",
    },
    {
      title: "Date Start",
      dataIndex: "dateStart",
      width: "15%",
      render: (dateStart: Date) => formatDate(dateStart),
    },
    {
      title: "Date End",
      dataIndex: "dateEnd",
      width: "15%",
      render: (dateEnd: Date) => formatDate(dateEnd),
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
              {statusStringMap[status].toUpperCase()}
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
        <a onClick={() => setDetail(deposit.reasId)}>View details</a>
      ),
      width: "15%",
    },
  ];

  const detailColumns: TableProps<DepositAmountUser>["columns"] = [
    {
      title: "No",
      width: "5%",
      render: (index: number) => index + 1,
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
      render: (depositAmount: number) => NumberFormat(depositAmount),
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
        const color = statusDepositUserColorMap[reas_Status] || "gray"; // Mặc định là màu xám nếu không có trong ánh xạ
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
      render: (_: any, record: DepositAmountUser) => {
        if (record.status === "Waiting_for_refund") {
          return (
            <Button
              onClick={() =>
                handleChangeStatus(
                  record.accountSignId,
                  record.reasId,
                  record.amount,
                  record.depositID
                )
              }
            >
              Refund
            </Button>
          );
        } else {
          return null;
        }
      },
      width: "10%",
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
              <h5>
                <strong>Real Estate Name: {reasName}</strong>
              </h5>
              <br />
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
              <div>
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
            dataSource={depositsList?.filter((reas: deposit) => {
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

export default AllDepositsList;
