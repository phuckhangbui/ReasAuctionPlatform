import { useFormik } from "formik";
import { useContext, useEffect, useState } from "react";
import { UserContext } from "../../../context/userContext";
import { getMemberProfile, updateMemberProfile } from "../../../api/member";
import toast from "react-hot-toast";

const majors: major[] = [
  { majorId: 1, majorName: "Education" },
  { majorId: 2, majorName: "Information Technology" },
  { majorId: 3, majorName: "Health and Medicine" },
  { majorId: 4, majorName: "Social Sciences" },
  { majorId: 5, majorName: "Engineering and Technology" },
  { majorId: 6, majorName: "Arts and Design" },
  { majorId: 7, majorName: "Business and Finance" },
  { majorId: 8, majorName: "Environment and Resources" },
  { majorId: 9, majorName: "Law" },
  { majorId: 10, majorName: "Agriculture and Farming" },
  { majorId: 11, majorName: "Freelancer" },
];

const bankTypes = ["NCB", "VISA", "MasterCard", "JCB", "EXIMBANK"];

const validate = (values: updatedMember) => {
  const errors: Partial<updatedMember> = {};

  if (!values.username) {
    errors.username = "Required";
  } else if (values.username.length > 20) {
    errors.username = "Username is too long";
  }

  if (!values.accountName) {
    errors.accountName = "Required";
  } else if (values.accountName.length > 20) {
    errors.accountName = "Account name is too long";
  }

  if (!values.phoneNumber) {
    errors.phoneNumber = "Required";
  } else if (!parseInt(values.phoneNumber)) {
    errors.phoneNumber = "Phone number must be numbers";
  } else if (!values.phoneNumber.startsWith("0")) {
    errors.phoneNumber = "Phone number must start with 0";
  } else if (values.phoneNumber.length > 10 || values.phoneNumber.length < 10) {
    errors.phoneNumber = "Phone number must be 10 in length";
  }

  if (!values.citizenIdentification) {
    errors.citizenIdentification = "Required";
  } else if (!parseInt(values.citizenIdentification)) {
    errors.citizenIdentification = "Citizen identification must be numbers";
  } else if (!values.citizenIdentification.startsWith("0")) {
    errors.citizenIdentification = "Citizen identification must start with 0";
  } else if (
    values.citizenIdentification.length > 12 ||
    values.citizenIdentification.length < 12
  ) {
    errors.citizenIdentification =
      "Citizen identification must be 12 in length";
  }

  if (!values.address) {
    errors.address = "Required";
  }

  if (!values.majorId) {
    errors.address = "Required";
  }

  // if (!values.bankingCode) {
  //   errors.address = "Required";
  // }

  if (!values.bankingNumber) {
    errors.address = "Required";
  } else if (!parseInt(values.bankingNumber)) {
    errors.bankingNumber = "Bank numbers must be numbers";
  }

  return errors;
};

const ProfilePage = () => {
  const { token } = useContext(UserContext);
  const [memberProfile, setMemberProfile] = useState<memberProfile>();

  useEffect(() => {
    try {
      const fetchMemberProfile = async () => {
        if (token) {
          const response = await getMemberProfile(token);
          console.log(response);
          if (response) {
            setMemberProfile(response);
            formik.setValues(response);
          }
        }
      };
      fetchMemberProfile();
    } catch (error) {
      console.log("Error: ", error);
    }
  }, []);

  const formik = useFormik({
    initialValues: {
      username: memberProfile?.username || "",
      accountName: memberProfile?.accountName || "",
      phoneNumber: memberProfile?.phoneNumber || "",
      citizenIdentification: memberProfile?.citizenIdentification || "",
      address: memberProfile?.address || "",
      majorId: memberProfile?.majorId || 1,
      bankingNumber: memberProfile?.bankingNumber || "",
      bankingCode: memberProfile?.bankingCode || "",
    } as updatedMember,
    validate,
    onSubmit: async (values) => {
      try {
        console.log(values);
        if (
          !values.username ||
          !values.accountName ||
          !values.phoneNumber ||
          !values.citizenIdentification ||
          !values.address ||
          !values.majorId ||
          !values.bankingNumber ||
          !values.bankingCode
        ) {
          toast.error("Some of the input are missing");
        } else {
          if (token) {
            const response = await updateMemberProfile(values, token);
            if (response === "Update success") {
              toast.success(response);
            }
          }
        }
      } catch (error) {
        console.log("Error: ", error);
      }
    },
  });

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    formik.handleSubmit();
  };

  return (
    <>
      <div className="pt-20">
        <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
          <div className="bg-white border border-gray-200 rounded-lg shadow mx-auto w-full px-10 py-5">
            <div className="text-center">
              <div className="text-gray-900  text-4xl font-bold">
                Your Profile
              </div>
              <div className="mt-2">
                View and update your profile to suit your needs here
              </div>
            </div>
            <form action="" onSubmit={handleSubmit}>
              <div className="grid grid-cols-4 gap-2 p-5">
                <div className="col-span-2">
                  <label
                    htmlFor="accountName"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white"
                  >
                    Account Name
                  </label>
                  <input
                    type="text"
                    id="accountName"
                    className={`${
                      formik.touched.accountName && formik.errors.accountName
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Account Name"
                    required
                    value={formik.values.accountName}
                    onBlur={formik.handleBlur}
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 20) {
                        formik.handleChange(e);
                      }
                    }}
                  />
                  {formik.touched.accountName && formik.errors.accountName ? (
                    <div className="text-red-700">
                      {formik.errors.accountName}
                    </div>
                  ) : (
                    <div className="text-transparent pointer-events-none ">
                      '
                    </div>
                  )}
                </div>
                <div className="col-span-2">
                  <label
                    htmlFor="username"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white"
                  >
                    User Name
                  </label>
                  <input
                    type="text"
                    id="username"
                    className={`${
                      formik.touched.username && formik.errors.username
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="User Name"
                    required
                    value={formik.values.username}
                    onBlur={formik.handleBlur}
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 20) {
                        formik.handleChange(e);
                      }
                    }}
                  />
                  {formik.touched.username && formik.errors.username ? (
                    <div className="text-red-700">{formik.errors.username}</div>
                  ) : (
                    <div className="text-transparent pointer-events-none ">
                      '
                    </div>
                  )}
                </div>
                <div className="col-span-2">
                  <label
                    htmlFor="phoneNumber"
                    className="block mb-1 text-md font-medium text-gray-900"
                  >
                    Phone Number
                  </label>
                  <input
                    type="text"
                    id="phoneNumber"
                    className={`${
                      formik.touched.phoneNumber && formik.errors.phoneNumber
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Phone Number"
                    required
                    value={formik.values.phoneNumber}
                    onBlur={formik.handleBlur}
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 10) {
                        formik.handleChange(e);
                      }
                    }}
                  />
                  {formik.touched.phoneNumber && formik.errors.phoneNumber ? (
                    <div className="text-red-700">
                      {formik.errors.phoneNumber}
                    </div>
                  ) : (
                    <div className="text-transparent pointer-events-none ">
                      '
                    </div>
                  )}
                </div>
                <div className="col-span-2">
                  <label
                    htmlFor="citizenIdentification"
                    className="block mb-1 text-md font-medium text-gray-900 "
                  >
                    Citizen Identification
                  </label>
                  <input
                    type="text"
                    id="citizenIdentification"
                    className={`${
                      formik.touched.citizenIdentification &&
                      formik.errors.citizenIdentification
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Citizen Identification"
                    required
                    value={formik.values.citizenIdentification}
                    onBlur={formik.handleBlur}
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 12) {
                        formik.setFieldValue("citizenIdentification", value);
                      }
                    }}
                  />
                  {formik.touched.citizenIdentification &&
                  formik.errors.citizenIdentification ? (
                    <div className="text-red-700">
                      {formik.errors.citizenIdentification}
                    </div>
                  ) : (
                    <div className="text-transparent pointer-events-none ">
                      '
                    </div>
                  )}
                </div>
                <div className="col-span-3">
                  <label
                    htmlFor="address"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white"
                  >
                    Address
                  </label>
                  <input
                    type="text"
                    id="address"
                    className={`${
                      formik.touched.address && formik.errors.address
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Address"
                    required
                    value={formik.values.address}
                    onBlur={formik.handleBlur}
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 50) {
                        formik.handleChange(e);
                      }
                    }}
                  />
                  {formik.touched.address && formik.errors.address ? (
                    <div className="text-red-700">{formik.errors.address}</div>
                  ) : (
                    <div className="text-transparent pointer-events-none ">
                      '
                    </div>
                  )}
                </div>
                <div className="col-span-1">
                  <label
                    htmlFor="major"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white "
                  >
                    Major
                  </label>
                  <select
                    id="major"
                    className="w-full p-3 outline-none text-sm rounded-lg bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue"
                    onChange={(e) => {
                      formik.setFieldValue("majorId", e.target.value);
                    }}
                    value={formik.values.majorId}
                    onBlur={formik.handleBlur}
                    required
                  >
                    {majors?.map((major) => (
                      <option value={major.majorId} key={major.majorId}>
                        {major.majorName}
                      </option>
                    ))}
                  </select>
                </div>
                <div className="col-span-3">
                  <label
                    htmlFor="bankingNumber"
                    className="block mb-1 text-md font-medium text-gray-900"
                  >
                    Banking Information{" "}
                    <span className="text-gray-500 text-sm">
                      (Incase of Refund)
                    </span>
                  </label>
                  <input
                    type="number"
                    id="bankingNumber"
                    className={`${
                      formik.touched.bankingNumber &&
                      formik.errors.bankingNumber
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Banking Type"
                    required
                    value={formik.values.bankingNumber}
                    onBlur={formik.handleBlur}
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 16) {
                        formik.handleChange(e);
                      }
                    }}
                  />
                  {formik.touched.bankingNumber &&
                  formik.errors.bankingNumber ? (
                    <div className="text-red-700">
                      {formik.errors.bankingNumber}
                    </div>
                  ) : (
                    <div className="text-transparent pointer-events-none ">
                      '
                    </div>
                  )}
                </div>
                <div className="col-span-1">
                  <div className="block mb-1 text-md font-medium text-transparent">
                    .
                  </div>
                  <select
                    className="w-full p-3 outline-none text-sm rounded-lg bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue"
                    onChange={(e) => {
                      formik.setFieldValue("bankingCode", e.target.value);
                    }}
                    value={formik.values.bankingCode}
                    onBlur={formik.handleBlur}
                    required
                  >
                    {bankTypes?.map((bankType) => (
                      <option value={bankType} key={bankType}>
                        {bankType}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
              <div className="flex justify-center p-4">
                <button className="bg-mainBlue px-5 py-2 text-white rounded-lg hover:bg-darkerMainBlue">
                  Change
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </>
  );
};

export default ProfilePage;
