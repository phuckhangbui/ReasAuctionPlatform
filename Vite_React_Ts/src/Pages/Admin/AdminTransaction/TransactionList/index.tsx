import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button, DatePicker, Descriptions, Table, TableProps, Tag } from "antd";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { NumberFormat } from "../../../../Utils/numberFormat";
import { UserContext } from "../../../../context/userContext";
import dayjs from "dayjs";
import {
  getTransaction,
  getTransactionDetail,
} from "../../../../api/transaction";

const statusStringMap: { [key: number]: string } = {
  0: "Success",
  1: "Error",
};

const statusTransactionColorMap: { [key: string]: string } = {
  Success: "green",
  Error: "red",
};

const TransactionList: React.FC = () => {
  const [search, _setSearch] = useState<searchTransaction>({
    dateExecutionFrom: "",
    dateExecutionTo: "",
  });
  const [transactionsList, setTransactionsList] = useState<transaction[]>();
  const [transactionDetail, setTransactionDetail] =
    useState<transactionDetail>();
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const { token } = useContext(UserContext);

  useEffect(() => {
    try {
      const fetchData = async () => {
        if (token) {
          if (search) {
            const response = await getTransaction(
              token,
              search.dateExecutionFrom,
              search.dateExecutionTo
            );
            setTransactionsList(response);
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

  const FormSearch = () => {
    const [dateExecutionFrom, setDateExecutionFrom] = useState<string | undefined>();
    const [dateExecutionTo, setDateExecutionTo] = useState<string | undefined>();
    const { RangePicker } = DatePicker;

    const handleDateRange = (
      dates: [dayjs.Dayjs | null, dayjs.Dayjs | null],
      dateStrings: [string, string]
    ) => {
      if (dates && dates.length === 2) {
        setDateExecutionFrom(dateStrings[0]);
        setDateExecutionTo(dateStrings[1]);
      }
    };

    const handleSearch = useCallback(async () => {
      try {
        if (token) {
          const response = await getTransaction(
            token,
            dateExecutionFrom,
            dateExecutionTo
          );
          setTransactionsList(response);
        }
      } catch (error) {
        console.log(error);
      }
    }, [dateExecutionFrom, dateExecutionTo, token]);

    return (
      <div>
        <RangePicker onChange={handleDateRange} />
        <Button onClick={handleSearch}>Search</Button>
      </div>
    );
  };

  const columns: TableProps<transaction>["columns"] = [
    {
      title: "No.",
      dataIndex: "transactionNo",
      width: "20%",
    },
    {
      title: "Status",
      dataIndex: "transactionStatus",
      width: "5%",
      render: (transactionStatus: number) => {
          const color =
            statusTransactionColorMap[statusStringMap[transactionStatus]];
          return (
            <Tag color={color} key={statusStringMap[transactionStatus]}>
              {statusStringMap[transactionStatus]}
            </Tag>
          );
      },
    },
    {
      title: "Money",
      dataIndex: "money",
      render: (money: number) => NumberFormat(money),
      width: "20%",
    },
    {
      title: "Type",
      dataIndex: "transactionType",
      width: "20%",
    },
    {
      title: "Date Exceuted",
      dataIndex: "dateExecution",
      width: "15%",
      render: (dateExecution: Date) => formatDate(dateExecution),
    },
    {
      title: "Actions",
      dataIndex: "operation",
      render: (_: any, transaction: transaction) => (
        <a onClick={() => viewDepositDetail(transaction.transactionId)}>
          View details
        </a>
      ),
      width: "15%",
    },
  ];
  const viewDepositDetail = (transactionId: number) => {
    try {
      const fetchDepositDetail = async () => {
        if (token && transactionId) {
          const response = await getTransactionDetail(token, transactionId);
          if (response) {
            setTransactionDetail(response);
          }
          setShowDetail(true);
        }
      };
      fetchDepositDetail();
    } catch (error) {
      console.log(error);
    }
  };

  const handleBackToList = () => {
    setShowDetail(false);
  };

  const ParseStatusString = (transactionStatus: number) => {
    const color =
      statusTransactionColorMap[statusStringMap[transactionStatus]];
    return (
      <Tag color={color} key={statusStringMap[transactionStatus]}>
        {statusStringMap[transactionStatus]}
      </Tag>
    );
};

  const renderBorderedItems = () => {
    const items = [
      {
        key: "1",
        label: "No.",
        children: transactionDetail?.transactionNo || "",
      },
      {
        key: "2",
        label: "Sender Name",
        children: transactionDetail?.accountSendName || "",
      },
      {
        key: "3",
        label: "Sender Banking Code",
        children: transactionDetail?.accountSendBankingCode || "",
      },
      {
        key: "4",
        label: "Sender Banking Account",
        children: transactionDetail?.accountSendBankingNumber || "",
      },
      {
        key: "5",
        label: "Receiver Name",
        children: transactionDetail?.accountReceiveName || "",
      },
      {
        key: "6",
        label: "Receiver Banking Code",
        children: transactionDetail?.accountReceiveBankingCode || "",
      },
      {
        key: "7",
        label: "Receiver Banking Account",
        children: transactionDetail?.accountReceiveBankingNumber || "",
      },
      {
        key: "8",
        label: "Real Estate Name",
        children: transactionDetail?.reasName || "",
      },
      {
        key: "9",
        label: "Status",
        children: transactionDetail?.transactionStatus //? ParseStatusString(transactionDetail?.transactionStatus),
      },
      {
        key: "10",
        label: "Money",
        children: transactionDetail?.money ? NumberFormat(transactionDetail?.money) : "",
      },
      {
        key: "11",
        label: "Type",
        children: transactionDetail?.transactionType || "",
      },
      {
        key: "12",
        label: "Date Executed",
        children: transactionDetail?.dateExecution
          ? formatDate(transactionDetail?.dateExecution)
          : "",
      },
    ];
    return items.map((item) => (
      <Descriptions.Item key={item.key} label={item.label}>
        {item.children}
      </Descriptions.Item>
    ));
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
              <Descriptions bordered title="Detail of Deposit">
                {/* <Table
                  columns={detailColumns}
                  dataSource={depositDetail}
                  bordered
                /> */}
                {renderBorderedItems()}
              </Descriptions>
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
          <Table columns={columns} dataSource={transactionsList} bordered />
        </div>
      )}
    </>
  );
};

export default TransactionList;
