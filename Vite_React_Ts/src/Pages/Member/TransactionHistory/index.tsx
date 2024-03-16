import { DatePicker, Select, Table, Tag } from "antd";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { NumberFormat } from "../../../Utils/numberFormat";
import { getTransactionHistory } from "../../../api/MoneyTransaction/MemberMoneyTransaction";
import { useContext } from "react";
import { UserContext } from "../../../context/userContext";

const TransactionHistory: React.FC = () => {
  const { token } = useContext(UserContext);
  const { userId } = useContext(UserContext);
  const [transaction, setTransaction] = useState<
    transactionHistory[] | undefined
  >([]);
  const fetchTransaction = async () => {
    if (userId && token) {
      let response = await getTransactionHistory(userId, token);
      setTransaction(response);
      console.log(
        transaction?.forEach((history) => {
          console.log(history.dateExecution);
        })
      );
    }
  };
  useEffect(() => {
    try {
      fetchTransaction();
    } catch (error) {
      console.log(error);
    }
  }, []);

  const handleChange = (value: string) => {
    console.log(`selected ${value}`);
  };

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
      title: "Transaction Type",
      dataIndex: "transactionType",
    },
    {
      title: "Status",
      dataIndex: "transactionStatus",
      render: (status: number) =>
        status == 0 ? (
          <Tag color="volcano">Error</Tag>
        ) : (
          <Tag color="green">Success</Tag>
        ),
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
    </>
  );
};

export default TransactionHistory;
