import { useFormik } from "formik";
import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { UserContext } from "../../../context/userContext";
import { createRealEstate, getRealEstateType } from "../../../api/realEstate";
import { DatePicker } from "antd";
import toast from "react-hot-toast";
import { useParams } from "react-router-dom";
import { memberRealEstateDetail } from "../../../api/memberRealEstate";
import dayjs from "dayjs";

const validate = (values: createRealEstate) => {
  const errors: Partial<validateRealEstate> = {};
  if (values.reasPrice < 0) {
    errors.reasPrice = "Price cannot be negative";
  } else if (!values.reasPrice || values.reasPrice == 0) {
    errors.reasPrice = "Required";
  } else if (isNaN(values.reasPrice)) {
    errors.reasPrice = "Price must be a number";
  } else if (values.reasPrice < 100000000) {
    errors.reasPrice = "Price must be more than 100.000.000";
  } else if (values.reasPrice > 100000000000) {
    errors.reasPrice = "Price must be less than 100.000.000.000 đ";
  } else if (!values.reasPrice.toString().endsWith("000")) {
    errors.reasPrice = "Price must end with 000 at the end";
  }

  if (!values.dateEnd) {
    errors.reasName = "Required";
  }

  return errors;
};

const UpdateRealEstatePage = () => {
  let { reasId } = useParams();
  const { token } = useContext(UserContext);
  const [_realEstateDetail, setRealEstateDetail] = useState<realEstateDetail>();
  const [formikDetail, setFormikDetail] = useState<createRealEstate>();
  const [realEstateTypes, setRealEstateTypes] = useState<realEstateType[]>();
  const [realEstateTypeName, setRealEstateTypeName] = useState<string>();
  const [tabStatus, setTabStatus] = useState<string>("information");
  const [noPhotoMessage, setNoPhotoMessage] = useState<boolean>(false);
  const [noInputMessage, setNoInputMessage] = useState<boolean>(false);
  const navigate = useNavigate();

  useEffect(() => {
    try {
      const fetchRealEstateDetail = async () => {
        if (reasId && token) {
          const response = await memberRealEstateDetail(
            parseInt(reasId),
            token
          );
          console.log(response);
          if (response) {
            setRealEstateDetail(response);
            const {
              reasName,
              reasAddress,
              reasPrice,
              reasArea,
              reasDescription,
              dateEnd,
              type_Id,
              photos,
              detail,
            } = response;

            setFormikDetail({
              reasName,
              reasAddress,
              reasPrice,
              reasArea,
              reasDescription,
              dateEnd,
              type_Reas: type_Id,
              photos: photos,
              detail,
            });
          }
        }
      };
      const fetchRealEstateTypes = async () => {
        const response = await getRealEstateType(token);
        setRealEstateTypes(response);
      };

      fetchRealEstateTypes();
      fetchRealEstateDetail();
    } catch (error) {
      console.log("Error: ", error);
    }
  }, []);

  useEffect(() => {
    console.log(formikDetail);
    if (formikDetail) {
      formik.setValues(formikDetail);
    }
  }, [formikDetail]);

  useEffect(() => {
    realEstateTypes?.map((realEstateType) => {
      if (realEstateType.typeReasId === formikDetail?.type_Reas) {
        if (realEstateType.typeName) {
          setRealEstateTypeName(realEstateType.typeName);
        }
      }
    });
  }, [realEstateTypes]);

  useEffect(() => {
    if (noPhotoMessage) {
      const timeout = setTimeout(() => {
        setNoPhotoMessage(false);
      }, 3000);

      return () => clearTimeout(timeout);
    }
  }, [noPhotoMessage]);

  useEffect(() => {
    if (noInputMessage) {
      const timeout = setTimeout(() => {
        setNoInputMessage(false);
      }, 3000);

      return () => clearTimeout(timeout);
    }
  }, [noInputMessage]);

  const formik = useFormik({
    initialValues: {
      reasName: formikDetail?.reasName || "",
      reasAddress: formikDetail?.reasAddress || "",
      reasPrice: formikDetail?.reasPrice || 100000000,
      reasArea: formikDetail?.reasArea || 100000,
      reasDescription: formikDetail?.reasDescription || "",
      dateEnd: formikDetail?.dateEnd || new Date(),
      type_Reas: formikDetail?.type_Reas || 1,
      photos: (formikDetail?.photos || []).map((photo) => ({
        reasPhotoUrl: photo.reasPhotoUrl,
      })),
      detail: {
        reas_Cert_Of_Land_Img_Front:
          formikDetail?.detail.reas_Cert_Of_Land_Img_Front || "",
        reas_Cert_Of_Land_Img_After:
          formikDetail?.detail.reas_Cert_Of_Land_Img_After || "",
        reas_Cert_Of_Home_Ownership:
          formikDetail?.detail.reas_Cert_Of_Home_Ownership || "",
        reas_Registration_Book:
          formikDetail?.detail.reas_Registration_Book || "",
        documents_Proving_Marital_Relationship:
          formikDetail?.detail.documents_Proving_Marital_Relationship || "",
        sales_Authorization_Contract:
          formikDetail?.detail.sales_Authorization_Contract || "",
      },
    } as createRealEstate,
    validate,
    onSubmit: async (values: any) => {
      try {
        console.log(values);
        const response = await createRealEstate(token, values);
        if (response) {
          formik.resetForm();
          navigate("/memberReas");
        }
      } catch (error) {
        console.log("Error: ", error);
      }
    },
  });

  const toggleTab = (index: string) => {
    setTabStatus(index);
  };

  const getActiveTabDetail = (index: string) => {
    return `${index === tabStatus ? "" : "hidden"}`;
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    formik.handleSubmit();
  };

  const handleChangeTab = () => {
    if (
      !formik.values.reasPrice ||
      !formik.values.dateEnd ||
      formik.errors.reasPrice
    ) {
      toast.error("You Are Missing Some Inputs");
    } else {
      toggleTab("reasPhoto");
    }
  };

  const formattedReasPrice = formik.values.reasPrice.toLocaleString("vi-VN", {
    style: "currency",
    currency: "VND",
    maximumFractionDigits: 3,
  });

  return (
    <div className="pt-20">
      <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
        <div className="bg-white border border-gray-200 rounded-lg shadow mx-auto w-full px-10 py-5">
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              Real Estate Request
            </div>
            <div className="mt-2">
              Send us your request to auction your house so we can help you can
              sell your estate at a profitable price
            </div>
          </div>
          <ol className="items-center w-full space-y-4 sm:flex sm:space-x-8 sm:space-y-0 rtl:space-x-reverse px-4 pt-5">
            <li
              className={`${
                tabStatus === "information" ? "text-mainBlue " : "text-gray-500"
              } space-x-2.5 rtl:space-x-reverse flex items-center`}
            >
              <span
                className={`${
                  tabStatus === "information"
                    ? "border-mainBlue"
                    : "border-gray-500"
                } flex items-center justify-center w-8 h-8 border  rounded-full shrink-0`}
              >
                1
              </span>
              <span>
                <h3
                  className={`${
                    tabStatus === "information" ? "font-bold" : "font-medium "
                  } leading-tight`}
                >
                  Real Estate Info
                </h3>
                <p className="text-sm">Enter real estate detail</p>
              </span>
            </li>

            <li
              className={`${
                tabStatus === "reasPhoto" ? "text-mainBlue " : "text-gray-500"
              } space-x-2.5 rtl:space-x-reverse flex items-center`}
            >
              <span
                className={`${
                  tabStatus === "reasPhoto"
                    ? "border-blue-600"
                    : "border-gray-500"
                } flex items-center justify-center w-8 h-8 border  rounded-full shrink-0`}
              >
                2
              </span>
              <span>
                <h3
                  className={`${
                    tabStatus === "reasPhoto" ? "font-bold" : "font-medium "
                  } leading-tight`}
                >
                  Land Certification
                </h3>
                <p className="text-sm">Upload certification</p>
              </span>
            </li>
          </ol>
          <form className="" onSubmit={handleSubmit}>
            <div className={getActiveTabDetail("information")}>
              <div className="grid grid-cols-4 gap-2 p-5">
                <div className="col-span-4">
                  <label
                    htmlFor="reasName"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white"
                  >
                    Title
                  </label>
                  <input
                    type="text"
                    id="reasName"
                    className={`${
                      formik.touched.reasName && formik.errors.reasName
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Real Estate Title"
                    required
                    value={formik.values.reasName}
                    onBlur={formik.handleBlur}
                    disabled
                  />
                </div>
                <div className="col-span-2">
                  <label
                    htmlFor="reasAddress"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white "
                  >
                    Address
                  </label>
                  <input
                    type="text"
                    id="reasAddress"
                    className={`${
                      formik.touched.reasAddress && formik.errors.reasAddress
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Address"
                    required
                    value={formik.values.reasAddress}
                    onBlur={formik.handleBlur}
                    disabled
                  />
                </div>
                <div className="col-span-1">
                  <label
                    htmlFor="reasArea"
                    className="block mb-1 text-md font-medium text-gray-900 "
                  >
                    Area <span className="text-sm text-gray-500">(m²)</span>
                  </label>
                  <input
                    type="number"
                    id="reasArea"
                    className={`${
                      formik.touched.reasArea && formik.errors.reasArea
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Area of Land"
                    required
                    value={formik.values.reasArea}
                    onBlur={formik.handleBlur}
                    disabled
                  />
                </div>
                <div className="col-span-1">
                  <label
                    htmlFor="reasPrice"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white "
                  >
                    Price <span className="text-sm text-gray-500">(VND)</span>
                  </label>
                  <input
                    type="number"
                    id="reasPrice"
                    className={`${
                      formik.touched.reasPrice && formik.errors.reasPrice
                        ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700  focus:ring-red-500/50 focus:border-red-500 "
                        : "bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    }focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg`}
                    placeholder="Price of Land"
                    required
                    value={formik.values.reasPrice}
                    onBlur={formik.handleBlur}
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 12) {
                        formik.handleChange(e);
                      }
                    }}
                  />
                  {formik.touched.reasPrice && formik.errors.reasPrice ? (
                    <div className="text-red-700">
                      {formik.errors.reasPrice}
                    </div>
                  ) : (
                    <div>{formattedReasPrice}</div>
                  )}
                </div>
                <div className="col-span-2">
                  <label
                    htmlFor="dateRange"
                    className=" mb-1 text-md font-medium text-gray-900 dark:text-white grid grid-cols-2"
                  >
                    <div>End Date</div>
                  </label>
                  {/* <RangePicker
                    id="dateRange"
                    className=" w-full p-2.5 outline-none text-sm rounded-lg bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    onChange={(dateStrings: any) => {
                      formik.setFieldValue("dateStart", dateStrings[0]);
                      formik.setFieldValue("dateEnd", dateStrings[1]);
                    }}
                    required
                  />
                </div> */}
                  <DatePicker
                    className="w-full p-2.5 outline-none text-sm rounded-lg bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue"
                    value={
                      formik.values.dateEnd
                        ? dayjs(formik.values.dateEnd)
                        : null
                    }
                    disabledDate={(current) =>
                      current && current < dayjs().startOf("day")
                    }
                    onChange={(_date, dateString) => {
                      formik.setFieldValue("dateEnd", dateString);
                    }}
                    format="YYYY-MM-DD"
                  />
                </div>
                <div className="col-span-2">
                  <label
                    htmlFor="type_Reas"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white "
                  >
                    Real Estate Type
                  </label>

                  {/* <select
                    id="type_Reas"
                    className="w-full p-3 outline-none text-sm rounded-lg bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue"
                    onChange={formik.handleChange}
                    value={formik.values.type_Reas}
                    onBlur={formik.handleBlur}
                    required
                  >
                    {realEstateTypes?.map((realEstateType) => (
                      <option
                        value={realEstateType.typeReasId}
                        key={realEstateType.typeReasId}
                      >
                        {realEstateType.typeName}
                      </option>
                    ))}
                  </select> */}
                  <input
                    type="type_Reas"
                    id="type_Reas"
                    className="bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue focus:border-1 focus:ring-2 block w-full p-2.5 outline-none text-sm rounded-lg"
                    placeholder="Price of Land"
                    required
                    value={realEstateTypeName}
                    onBlur={formik.handleBlur}
                    disabled
                  />
                </div>
                <div className="col-span-4">
                  <label
                    htmlFor="reasDescription"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white "
                  >
                    Description
                  </label>
                  <div
                    dangerouslySetInnerHTML={{
                      __html: formik.values?.reasDescription || "",
                    }}
                    className="mt-"
                  ></div>
                </div>
                <div className="">
                  <div className="block mb-1 text-md font-medium text-gray-900 col-span-1">
                    Real Estate Pictures
                  </div>
                  {formik.values.photos ? (
                    formik.values.photos.map((photo: any, index: any) => (
                      <div className="col-span-1 h-64 rounded-lg">
                        <img
                          key={index}
                          src={photo.reasPhotoUrl}
                          alt={`Uploaded ${index + 1}`}
                          className="w-full h-full object-fill"
                        />
                      </div>
                    ))
                  ) : (
                    <div></div>
                  )}
                </div>
              </div>
              <div className="p-4 flex items-center justify-center ">
                <div
                  className="bg-mainBlue px-5 py-2 text-white rounded-lg hover:bg-darkerMainBlue hover:cursor-pointer"
                  onClick={() => handleChangeTab()}
                >
                  Next
                </div>
              </div>
            </div>

            <div className={getActiveTabDetail("reasPhoto")}>
              <div className="px-4 pt-1">
                <div
                  className=" bg-transparent md:bg-transparent sm:bg-white sm:bg-opacity-60 rounded-3xl text-sm w-10 h-10 ms-auto inline-flex justify-center items-center "
                  data-modal-hide="default-modal"
                  onClick={() => toggleTab("information")}
                >
                  <svg
                    className="w-6 h-6  sm:text-black sm:hover:text-mainBlue "
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 14 10"
                  >
                    <path
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      d="M13 5H1m0 0 4 4M1 5l4-4"
                    />
                  </svg>
                  <span className="sr-only">Go back</span>
                </div>
              </div>

              <div className="grid grid-cols-3 gap-4 p-5">
                <div>
                  <div className="text-center text-lg">Front Certificate</div>
                  {formik.values.detail.reas_Cert_Of_Land_Img_Front && (
                    <div className="flex items-center justify-center w-full h-64">
                      <img
                        className="text-transparent w-full h-full object-fill rounded-lg"
                        src={formik.values.detail.reas_Cert_Of_Land_Img_Front}
                      />
                    </div>
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">Back Certificate</div>
                  {formik.values.detail.reas_Cert_Of_Land_Img_After && (
                    <div className="flex items-center justify-center w-full h-64">
                      <img
                        className="text-transparent w-full h-full object-fill rounded-lg"
                        src={formik.values.detail.reas_Cert_Of_Land_Img_After}
                      />
                    </div>
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">
                    Ownership Certificate
                  </div>
                  {formik.values.detail.reas_Cert_Of_Home_Ownership && (
                    <div className="flex items-center justify-center w-full h-64">
                      <img
                        className="text-transparent w-full h-full object-fill rounded-lg"
                        src={formik.values.detail.reas_Cert_Of_Home_Ownership}
                      />
                    </div>
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">Registration Book</div>
                  {formik.values.detail.reas_Registration_Book && (
                    <div className="flex items-center justify-center w-full h-64">
                      <img
                        className="text-transparent w-full h-full object-fill rounded-lg"
                        src={formik.values.detail.reas_Registration_Book}
                      />
                    </div>
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">
                    Relationship Document
                  </div>
                  {formik.values.detail
                    .documents_Proving_Marital_Relationship && (
                    <div className="flex items-center justify-center w-full h-64">
                      <img
                        className="text-transparent w-full h-full object-fill rounded-lg"
                        src={
                          formik.values.detail
                            .documents_Proving_Marital_Relationship
                        }
                      />
                    </div>
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">
                    Authorization Contract
                  </div>
                  {formik.values.detail.sales_Authorization_Contract && (
                    <div className="flex items-center justify-center w-full h-64">
                      <img
                        className="text-transparent w-full h-full object-fill rounded-lg"
                        src={formik.values.detail.sales_Authorization_Contract}
                      />
                    </div>
                  )}
                </div>
              </div>
              <div className="flex justify-center p-4">
                <button className="bg-mainBlue px-5 py-2 text-white rounded-lg hover:bg-darkerMainBlue">
                  Re-Up
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default UpdateRealEstatePage;
