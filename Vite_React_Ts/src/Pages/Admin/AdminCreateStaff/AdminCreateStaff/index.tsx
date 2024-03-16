import { Descriptions, Button, Input, notification, Tooltip  } from "antd";
import { useState, useEffect } from "react";
import { addNewStaff } from "../../../../api/newStaff";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";

const AdminAddStaff: React.FC = () => {
  const [phoneNumberError, setPhoneNumberError] = useState<boolean>(false);
  const [citizenIdentificationError, setCitizenIdentificationError] =
    useState<boolean>(false);
  const [staffData, setStaffData] = useState<NewStaff>({
    accountEmail: "",
    accountName: "",
    address: "",
    citizen_identification: "",
    passwordHash: "",
    phoneNumber: "",
    username: "",
  });
  const [disableCreate, setDisableCreate] = useState<boolean>(true); // State để lưu trữ dữ liệu nhân viên
  const { token } = useContext(UserContext);

  const fetchCreateStaff = async (NewStaff: NewStaff) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await addNewStaff(NewStaff, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching add staff:", error);
    }
  };
  
  const openNotificationWithIcon = (type: 'success' | 'error', description: string) => {
    notification[type]({
      message: "Notification Title",
      description: description,
    });
  };

  const createNewStaff = async () => {
      const response = await fetchCreateStaff(staffData);
      if (response != undefined && response) {
        if (response.statusCode == "MSG04") {
          openNotificationWithIcon("success", response.message);
        } else {
          openNotificationWithIcon("error", "Something went wrong when executing operation. Please try again!");
        }
    }
  };

  const handleChange = (fieldName: keyof NewStaff, value: string) => {
    setStaffData((prevData) => ({
      ...prevData,
      [fieldName]: value,
    }));
  };
  const handleBlur = (fieldName: keyof NewStaff, value: string) => {
    if (fieldName === "phoneNumber") {
      if (value.length !== 10) {
        setPhoneNumberError(true);
      } else {
        setPhoneNumberError(false);
      }
    }

    if (fieldName === "citizen_identification") {
      if (value.length !== 12) {
        setCitizenIdentificationError(true);
      } else {
        setCitizenIdentificationError(false);
      }
    }
  };
  useEffect(() => {
    // Kiểm tra trạng thái lỗi của các trường và cập nhật trạng thái vô hiệu hóa nút "Create"
    setDisableCreate(phoneNumberError || citizenIdentificationError);
  }, [phoneNumberError, citizenIdentificationError]);

  const items = [
    {
      key: "1",
      label: "Account Name",
      children: (
        <Input
          placeholder="Enter your account name"
          onChange={(e) => handleChange("accountName", e.target.value)}
        />
      ),
      span: 3,
    },
    {
      key: "2",
      label: "Username",
      children: (
        <Input
          placeholder="Enter your username"
          onChange={(e) => handleChange("username", e.target.value)}
        />
      ),
      span: 3,
    },
    {
      key: "3",
      label: "Password",
      children: (
        <Input
          placeholder="Enter your password"
          onChange={(e) => handleChange("passwordHash", e.target.value)}
        />
      ),
      span: 3,
    },
    {
      key: "4",
      label: "Account Email",
      children: (
        <Input
          placeholder="Enter your email"
          onChange={(e) => handleChange("accountEmail", e.target.value)}
        />
      ),
      span: 3,
    },
    {
      key: "5",
      label: "Phone Number",
      children: (
        <Tooltip title={phoneNumberError ? "Phone number must be 10 characters" : ""}>
          <Input
            placeholder="Enter your phone"
            onChange={(e) => handleChange("phoneNumber", e.target.value)}
            onBlur={(e) => handleBlur("phoneNumber", e.target.value)}
            className={phoneNumberError ? "error-input" : ""} // Thêm class error-input khi có lỗi
          />
        </Tooltip>
      ),
      span: 3,
    },
    {
      key: "6",
      label: "Citizen Identification",
      children: (
        <Tooltip title={citizenIdentificationError ? "Citizen identification must be 12 characters" : ""}>
          <Input
            placeholder="Enter your citizen identification"
            onChange={(e) => handleChange("citizen_identification", e.target.value)}
            onBlur={(e) => handleBlur("citizen_identification", e.target.value)}
            className={citizenIdentificationError ? "error-input" : ""} // Thêm class error-input khi có lỗi
          />
        </Tooltip>
      ),
      span: 3,
    },
    {
      key: "7",
      label: "Address",
      children: (
        <Input
          placeholder="Enter your address"
          onChange={(e) => handleChange("address", e.target.value)}
        />
      ),
      span: 3,
    },
  ];
  return (
    <>
      <Descriptions bordered title="Add new Staff" items={items}></Descriptions>
      <br />
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Button onClick={createNewStaff} disabled={disableCreate}>Create Staff</Button>
      </div>
    </>
  );
};
export default AdminAddStaff;
