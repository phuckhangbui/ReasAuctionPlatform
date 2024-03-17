import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { Table, TableProps, Descriptions, Button, notification } from "antd";
import { useState, useEffect } from "react";
import {
  ruleUpdate,
  getRuleHome,
  getRuleAdminById,
} from "../../../../api/rule";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

const AdminRuleList: React.FC = () => {
  const [rulesData, setRuleData] = useState<Rules[]>();
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [updatedContent, setUpdatedContent] = useState<string>("");
  const [ruleDetailData, setRuleDetailData] = useState<Rule>();
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const [ruleID, setRuleId] = useState<Number | undefined>();
  const [initialContent, setInitialContent] = useState<string>("");
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

  const fetchUpdateRule = async (ruleUpdateData: RuleUpdate) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await ruleUpdate(ruleUpdateData, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching update rule:", error);
    }
  };

  useEffect(() => {
    if (ruleDetailData) {
      setInitialContent(ruleDetailData.content || "");
    }
  }, [ruleDetailData]);

  const fetchRuleDetail = async (
    ruleId: Number | undefined,
    token: string | undefined
  ) => {
    try {
      if (token) {
        let data: Rule | undefined;
        data = await getRuleAdminById(ruleId, token);
        setRuleDetailData(data);
        setRuleId(ruleId);
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching rule detail:", error);
    }
  };

  const fetchRuleList = async () => {
    try {
      if (token) {
        let data: Rules[] | undefined;
        data = await getRuleHome(token);
        setRuleData(data);
      }
    } catch (error) {
      console.error("Error fetching rule list:", error);
    }
  };

  useEffect(() => {
    fetchRuleList();
  }, [token]);

  const viewDetail = (RuleId: number) => {
    fetchRuleDetail(RuleId, token);
  };

  const columns: TableProps<Rules>["columns"] = [
    {
      title: "Tilte",
      dataIndex: "title",
      width: "50%",
    },
    {
      title: "Date Create",
      dataIndex: "dateCreated",
      width: "15%",
      render: (date_Created: Date) => formatDate(date_Created),
    },
    {
      title: "Actions",
      dataIndex: "operation",
      render: (_: any, rule: Rules) => (
        <a onClick={() => viewDetail(rule.ruleId)}>View details</a>
      ),
      width: "15%",
    },
  ];

  const renderBorderedItems = () => {
    const items = [
      {
        key: "1",
        label: "Title",
        children: ruleDetailData?.title || "",
        span: 2,
      },
      {
        key: "2",
        label: "DateCreate",
        children: ruleDetailData ? formatDate(ruleDetailData.dateCreated) : "",
        span: 1
      },
      {
        key: "3",
        label: "Content",
        children: isEditing ? (
          <CKEditor
            editor={ClassicEditor}
            data={initialContent}
            onChange={(_e, editor) => setUpdatedContent(editor.getData())}
          />
        ) : (
          <div dangerouslySetInnerHTML={{ __html: initialContent }}></div>
        ),
        span: 3,
      },
    ];
    return items.map(item => (
        <Descriptions.Item key={item.key} label={item.label} span={item.span}>
          {item.children}
        </Descriptions.Item>
      ));
  };

  const EditRule = () => {
    setIsEditing(true);

    fetchRuleDetail(ruleID, token);
  };

  const handleUpdate = () => {
    setIsEditing(false);
    let newsContent1;
    if (updatedContent !== "") {
      newsContent1 = updatedContent;
    } else {
      newsContent1 = initialContent;
    }

    const updatedRuleData1: RuleUpdate = {
      idRule: ruleID,
      content: newsContent1,
    };
    getMessage(updatedRuleData1);
  };

  const handleBackToList = () => {
    setShowDetail(false);
    setIsEditing(false);
    setUpdatedContent("");
    fetchRuleList();
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

  const getMessage = async (updatedRuleData: RuleUpdate) => {
    const response = await fetchUpdateRule(updatedRuleData);
    if (response !== undefined && response) {
      if (response.statusCode === "MSG03") {
        openNotificationWithIcon("success", response.message);
      } else {
        openNotificationWithIcon(
          "error",
          "Something went wrong when executing operation. Please try again!"
        );
      }
      fetchRuleDetail(ruleID, token);
    }
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
            {isEditing ? (
              <Button onClick={handleUpdate}>Save</Button> // Thêm nút Save khi đang trong chế độ sửa
            ) : (
              <Button onClick={EditRule}>Edit</Button> // Thêm nút Edit khi không trong chế độ sửa
            )}
          </div>
          <br />
          <Descriptions bordered title="Detail of Term">
            {renderBorderedItems()}
          </Descriptions>
        </div>
      ) : (
        <div>
          {/* Bảng danh sách */}
          <h4>
            <strong>List Term</strong>
          </h4>
          <br />
          <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
            <div className="w-full md:w-72 flex flex-row justify-start">
            </div>
          </div>
          <Table columns={columns} dataSource={rulesData} bordered />
        </div>
      )}
    </>
  );
};

export default AdminRuleList;
