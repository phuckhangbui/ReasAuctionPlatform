import { DatePicker, Modal, Table, Tag } from "antd";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { NumberFormat } from "../../../Utils/numberFormat";
import {
  getTransactionDetail,
  getTransactionHistory,
} from "../../../api/MoneyTransaction/MemberMoneyTransaction";
import { useContext } from "react";
import { UserContext } from "../../../context/userContext";
import { Button } from "@material-tailwind/react";

const TransactionHistory: React.FC = () => {
  const { token } = useContext(UserContext);
  const { userId } = useContext(UserContext);
  const [open, setOpen] = useState(false);
  const [transaction, setTransaction] = useState<
    transactionHistory[] | undefined
  >([]);
  const [transactionDetail, setTransactioDetail] = useState<
    TransactionDetail | undefined
  >();
  const [data, setData] = useState<TransactionDetail[]>([]);

  const fetchTransaction = async () => {
    if (userId && token) {
      let response = await getTransactionHistory(userId, token);
      setTransaction(response);
    }
  };

  const fetchTransactionDetail = async (transactionId: number) => {
    if (userId && token) {
      let response = await getTransactionDetail(userId, transactionId, token);
      setTransactioDetail(response);
    }
  };

  useEffect(() => {
    if (transactionDetail) {
      setData([transactionDetail]); // Set data with transactionDetail
      console.log(data);
    }
  }, [transactionDetail]);

  useEffect(() => {
    try {
      fetchTransaction();
    } catch (error) {
      console.log(error);
    }
  }, []);

  const handleRangeChange = (dates: any) => {
    console.log("Transaction: " + transaction);
    if (!dates) {
      fetchTransaction();
    }

    if (transaction !== undefined) {
      const startDate = dates[0] ? dayjs(dates[0]).startOf("day") : null;
      const endDate = dates[1] ? dayjs(dates[1]).endOf("day") : null;
      const filteredTransactions = transaction.filter((transactionItem) => {
        const transactionDate = dayjs(transactionItem.dateExecution);
        const isAfterStartDate =
          !startDate || transactionDate.isAfter(startDate);
        const isBeforeEndDate = !endDate || transactionDate.isBefore(endDate);

        // Log the result of isAfter and isBefore for debugging
        console.log("isAfter:", isAfterStartDate);
        console.log("isBefore:", isBeforeEndDate);

        // Return true if the transaction date is within the specified range
        return isAfterStartDate && isBeforeEndDate;
      });

      console.log("Filtered Transactions: ", filteredTransactions);

      setTransaction(filteredTransactions); // Update state with filtered transactions
    } else {
      console.log("undified");
    }
  };
  const { RangePicker } = DatePicker;

  const handleClicked = (transactionId: number) => {
    setOpen(true);
    fetchTransactionDetail(transactionId);
  };

  const columns = [
    {
      title: "Transaction No",
      dataIndex: "transactionNo",
    },
    {
      title: "Money",
      dataIndex: "money",
      render: (money: number) => NumberFormat(money),
    },
    {
      title: "Date Execution",
      dataIndex: "dateExecution",
      render: (date: Date) => dayjs(date).format("DD MMM YYYY HH:mm:ss"),
    },
    {
      title: "Status",
      dataIndex: "transactionStatus",
      render: (status: number) =>
        status == 0 ? (
          <Tag color="green">Success</Tag>
        ) : (
          <Tag color="volcano">Error</Tag>
        ),
    },
    {
      title: "",
      dataIndex: "operation",
      render: (_: any, record: transactionHistory) => (
        <Button
          variant="text"
          onClick={() => handleClicked(record.transactionId)}
        >
          View detail
        </Button>
      ),
      width: "10%",
    },
  ];

  const columnDetail = [
    {
      title: "Transaction No",
      dataIndex: "transactionNo",
    },
    {
      title: "Reas name",
      dataIndex: "reasName",
    },
    {
      title: "Send Name",
      dataIndex: "accountSendName",
    },
    {
      title: "Receive Name",
      dataIndex: "accountReceiveName",
    },
    {
      title: "Transaction Type",
      dataIndex: "transactionType",
    },
    {
      title: "Money",
      dataIndex: "money",
      render: (money: number) => NumberFormat(money),
    },
  ];

  return (
    <>
      <div className="pt-20">
        <div className="container w-full mx-auto">
          <div className="flex justify-between">
            <div className="w-1/2">
              <RangePicker onChange={(value) => handleRangeChange(value)} />
            </div>
          </div>
        </div>
      </div>{" "}
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <Table
            dataSource={transaction}
            columns={columns}
            rowKey="transactionNo"
          />
        </div>
      </div>
      <Modal
        centered
        open={open}
        onOk={() => setOpen(false)}
        onCancel={() => setOpen(false)}
        width={1000}
      >
        <Table
          dataSource={data}
          columns={columnDetail}
          rowKey="transactionId"
        />
      </Modal>
    </>
  );
};

export default TransactionHistory;
