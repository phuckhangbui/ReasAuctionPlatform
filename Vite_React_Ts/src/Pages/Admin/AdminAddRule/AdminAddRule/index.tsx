import { Descriptions, Button, Input, notification } from "antd";
import { useState} from "react";
import { addRule } from "../../../../api/rule";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

const AdminAddRule: React.FC = () => {
  const [ruleData, setRuleData] = useState<RuleAdd>({
    content: "",
    title: ""
  });
  const { token } = useContext(UserContext);

  const fetchCreateRule = async (rule: RuleAdd) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await addRule(rule, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching add rule:", error);
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

  const createRule = async () => {
    const response = await fetchCreateRule(ruleData);
    if (response != undefined && response) {
      if (response.statusCode == "MSG18") {
        openNotificationWithIcon("success", response.message);
      } else {
        openNotificationWithIcon(
          "error",
          "Something went wrong when executing operation. Please try again!"
        );
      }
    }

  };


  const handleChange = (fieldName: keyof RuleAdd, value: string) => {
    setRuleData((prevData) => ({
      ...prevData,
      [fieldName]: value,
    }));
  };
  const renderBorderedItems = () => {
  const items = [
    {
      key: "1",
      label: "Title",
      children: (
        <div>
          <Input
            placeholder="Enter title"
            onChange={(e) => handleChange("title", e.target.value)}
          />
        </div>
      ),
      span: 3
    },
    {
      key: "2",
      label: "Content",
      children: (
        <CKEditor
          editor={ClassicEditor}
          onChange={(_event, editor) => {
            const data = editor.getData();
            handleChange("content", data);
          }}
        />
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
  return (
    <>
      <Descriptions bordered title="Add Term">{renderBorderedItems()}</Descriptions>
      <br />
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Button onClick={createRule}>
          Create Term
        </Button>
      </div>
    </>
  );
};
export default AdminAddRule;
