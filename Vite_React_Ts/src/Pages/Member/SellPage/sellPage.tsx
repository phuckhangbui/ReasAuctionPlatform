import { useFormik } from "formik";
import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { UserContext } from "../../../context/userContext";
import { createRealEstate, getRealEstateType } from "../../../api/realEstate";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { DatePicker } from "antd";
import CloudinaryUploadWidget, {
  CloudinaryConfig,
} from "../../../Config/cloudinary";
import toast from "react-hot-toast";

const validate = (values: createRealEstate) => {
  const errors: Partial<validateRealEstate> = {};
  if (!values.reasName) {
    errors.reasName = "Required";
  } else if (values.reasName.length < 30) {
    errors.reasName = "Title is too short!";
  }

  if (values.reasArea < 0) {
    errors.reasArea = "Area cannot be negative";
  } else if (!values.reasArea || values.reasArea == 0) {
    errors.reasArea = "Required";
  } else if (isNaN(values.reasArea)) {
    errors.reasArea = "Area must be a number";
  } else if (values.reasArea < 100) {
    errors.reasArea = "Are of land must be more than 100";
  }

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
  } else if (!values.reasPrice.toString().endsWith('000')) {
    errors.reasPrice = "Price must end with 000 at the end";
  }

  if (!values.reasAddress) {
    errors.reasAddress = "Required";
  }

  if (!values.reasDescription) {
    errors.reasDescription = "Required";
  }

  if (!values.type_Reas) {
    errors.type_Reas = "Required";
  }

  if (!values.dateStart) {
    errors.dateStart = "Required";
  }

  if (!values.dateEnd) {
    errors.reasName = "Required";
  }

  return errors;
};

const SellPage = () => {
  const { token, userId, userAccountName } = useContext(UserContext);
  const [realEstateTypes, setRealEstateTypes] = useState<realEstateType[]>();
  const [uploadedImages, setUploadedImages] = useState<string[]>([]);
  const [_publicId, setPublicId] = useState<string>("");
  const [_photoFront, setPhotoFront] = useState<string>("");
  const [_photoBack, setPhotoBack] = useState<string>("");
  const [_photoOwnership, setPhotoOwnership] = useState<string>("");
  const [_photoBook, setPhotoBook] = useState<string>("");
  const [_photoDocu, setPhotoDocu] = useState<string>("");
  const [_photoContract, setPhotoContract] = useState<string>("");
  const [tabStatus, setTabStatus] = useState<string>("information");
  const [noPhotoMessage, setNoPhotoMessage] = useState<boolean>(false);
  const [noInputMessage, setNoInputMessage] = useState<boolean>(false);
  const navigate = useNavigate();
  const { RangePicker } = DatePicker;

  useEffect(() => {
    try {
      const fetchRealEstateTypes = async () => {
        const response = await getRealEstateType(token);
        setRealEstateTypes(response);
      };
      fetchRealEstateTypes();
    } catch (error) {
      console.log("Error: ", error);
    }
  }, []);

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
      reasName: "",
      reasAddress: "",
      reasPrice: 100000000,
      reasArea: 100000,
      reasDescription: "",
      dateStart: new Date(),
      dateEnd: new Date(),
      type_Reas: 1,
      photos: [
        {
          reasPhotoUrl: "",
        },
      ],
      detail: {
        reas_Cert_Of_Land_Img_Front: "",
        reas_Cert_Of_Land_Img_After: "",
        reas_Cert_Of_Home_Ownership: "",
        reas_Registration_Book: "",
        documents_Proving_Marital_Relationship: "",
        sales_Authorization_Contract: "",
      },
    } as createRealEstate,
    validate,
    onSubmit: async (values) => {
      try {
        console.log(values);
        if (
          !values.detail.reas_Cert_Of_Land_Img_Front ||
          !values.detail.reas_Cert_Of_Land_Img_After ||
          !values.detail.reas_Cert_Of_Home_Ownership ||
          !values.detail.reas_Registration_Book ||
          !values.detail.documents_Proving_Marital_Relationship ||
          !values.detail.sales_Authorization_Contract ||
          !values.photos.length
        ) {
          toast.error("Photos of Real Estate Are Required");
        } else {
          const response = await createRealEstate(token, values);
          if (response) {
            formik.resetForm();
            navigate("/memberReas");
          }
        }
      } catch (error) {
        console.log("Error: ", error);
      }
    },
  });

  const [uwConfig] = useState<CloudinaryConfig>({
    cloudName: "dqpsvl3nu",
    uploadPreset: "i0yxovxe",
    folder: `${userId || ""}-${userAccountName || ""}`,
  });

  const handleImageUpload = (imageUrl: string) => {
    setUploadedImages((prevImages) => [...prevImages, imageUrl]);
  };

  useEffect(() => {
    const updatedPhotos = uploadedImages.map((imageUrl) => ({
      reasPhotoUrl: imageUrl,
    }));
    formik.setFieldValue("photos", updatedPhotos);
  }, [uploadedImages]);

  const toggleTab = (index: string) => {
    setTabStatus(index);
  };

  const getActiveTabDetail = (index: string) => {
    return `${index === tabStatus ? "" : "hidden"}`;
  };

  const resetPhotosList = () => {
    setUploadedImages([]);
    formik.setFieldValue("photos", []);
  };

  const handleCertUpload = (imageUrl: string, index: string) => {
    switch (index) {
      case "front":
        setPhotoFront(imageUrl);
        formik.setFieldValue("detail.reas_Cert_Of_Land_Img_Front", imageUrl);
        break;
      case "back":
        setPhotoBack(imageUrl);
        formik.setFieldValue("detail.reas_Cert_Of_Land_Img_After", imageUrl);
        break;
      case "ownership":
        setPhotoOwnership(imageUrl);
        formik.setFieldValue("detail.reas_Cert_Of_Home_Ownership", imageUrl);
        break;
      case "book":
        setPhotoBook(imageUrl);
        formik.setFieldValue("detail.reas_Registration_Book", imageUrl);
        break;
      case "docu":
        setPhotoDocu(imageUrl);
        formik.setFieldValue(
          "detail.documents_Proving_Marital_Relationship",
          imageUrl
        );
        break;
      case "contract":
        setPhotoContract(imageUrl);
        formik.setFieldValue("detail.sales_Authorization_Contract", imageUrl);
        break;
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    formik.handleSubmit();
  };

  const handleChangeTab = () => {
    if (
      !formik.values.reasName ||
      !formik.values.reasAddress ||
      !formik.values.reasPrice ||
      !formik.values.reasArea ||
      !formik.values.reasDescription ||
      !formik.values.dateStart ||
      !formik.values.dateEnd ||
      !formik.values.type_Reas
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
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 80) {
                        formik.handleChange(e);
                      }
                    }}
                  />
                  {formik.touched.reasName && formik.errors.reasName ? (
                    <div className="text-red-700">{formik.errors.reasName}</div>
                  ) : (
                    <div className="text-white pointer-events-none ">'</div>
                  )}
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
                    onChange={formik.handleChange}
                  />
                  {formik.touched.reasAddress && formik.errors.reasAddress ? (
                    <div className="text-red-700">
                      {formik.errors.reasAddress}
                    </div>
                  ) : (
                    <div className="text-white pointer-events-none ">'</div>
                  )}
                </div>
                <div className="col-span-1">
                  <label
                    htmlFor="reasArea"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white "
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
                    onChange={(e) => {
                      const value = e.target.value;
                      if (value.length <= 6) {
                        formik.handleChange(e);
                      }
                    }}
                  />
                  {formik.touched.reasArea && formik.errors.reasArea ? (
                    <div className="text-red-700">{formik.errors.reasArea}</div>
                  ) : (
                    <div className="text-white pointer-events-none ">'</div>
                  )}
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
                      // if (value.length >= 9) {
                      //   formik.handleChange(e);
                      // }
                    }}
                  />
                  {formik.touched.reasPrice &&
                  formik.errors.reasPrice ? (
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
                    <div>Start Date</div>
                    <div>End Date</div>
                  </label>
                  <RangePicker
                    id="dateRange"
                    className=" w-full p-2.5 outline-none text-sm rounded-lg bg-gray-50 border border-gray-300 text-gray-900 focus:ring-mainBlue/30 focus:border-mainBlue "
                    onChange={(dateStrings: any) => {
                      formik.setFieldValue("dateStart", dateStrings[0]);
                      formik.setFieldValue("dateEnd", dateStrings[1]);
                    }}
                    required
                  />
                  <div className="grid grid-cols-2">
                    {formik.touched.reasDescription &&
                    formik.errors.reasDescription ? (
                      <div className="text-red-700">
                        {formik.errors.reasDescription}
                      </div>
                    ) : (
                      <div className="text-white ">'</div>
                    )}
                    {formik.touched.reasDescription &&
                    formik.errors.reasDescription ? (
                      <div className="text-red-700">
                        {formik.errors.reasDescription}
                      </div>
                    ) : (
                      <div className="text-white pointer-events-none ">'</div>
                    )}
                  </div>
                </div>
                <div className="col-span-2">
                  <label
                    htmlFor="type_Reas"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white "
                  >
                    Real Estate Type
                  </label>
                  <select
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
                  </select>
                </div>
                <div className="col-span-4 h-120">
                  <label
                    htmlFor="reasDescription"
                    className="block mb-1 text-md font-medium text-gray-900 dark:text-white "
                  >
                    Description
                  </label>
                  <CKEditor
                    id="reasDescription"
                    editor={ClassicEditor}
                    onChange={(_event: any, editor: any) => {
                      const data = editor.getData();
                      formik.setFieldValue("reasDescription", data);
                    }}
                    data={formik.values.reasDescription}
                    onBlur={formik.handleBlur}
                    onReady={(editor: any) => {
                      // You can store the "editor" and use when it is needed.
                      // console.log("Editor is ready to use!", editor);
                      editor.editing.view.change((writer: any) => {
                        writer.setStyle(
                          "height",
                          "400px",
                          editor.editing.view.document.getRoot()
                        );
                      });
                    }}
                  />
                  {formik.touched.reasDescription &&
                  formik.errors.reasDescription ? (
                    <div className="text-red-700">
                      {formik.errors.reasDescription}
                    </div>
                  ) : (
                    <div className="text-white pointer-events-none ">'</div>
                  )}
                </div>
                <div className="block mb-1 text-md font-medium text-gray-900 col-span-3">
                  Real Estate Pictures
                </div>
                <div className="col-span-1 flex justify-end">
                  {uploadedImages.length !== 0 ? (
                    <div
                      className="bg-red-700 px-3 py-1 text-white rounded-lg hover:bg-red-800 hover:cursor-pointer"
                      onClick={() => {
                        resetPhotosList();
                      }}
                    >
                      Reset List
                    </div>
                  ) : (
                    <></>
                  )}
                </div>
                <div className="col-span-1 ">
                  {realEstateTypes && (
                    <CloudinaryUploadWidget
                      uwConfig={uwConfig}
                      setPublicId={setPublicId}
                      setUploadedUrl={(imageUrl) => {
                        handleImageUpload(imageUrl);
                      }}
                      notList={false}
                    />
                  )}
                </div>
                {uploadedImages ? (
                  uploadedImages.map((image, index) => (
                    <div className="col-span-1 h-64 rounded-lg">
                      <img
                        key={index}
                        src={image}
                        alt={`Uploaded ${index + 1}`}
                        className="w-full h-full object-fill"
                      />
                    </div>
                  ))
                ) : (
                  <div></div>
                )}
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
                  {realEstateTypes && (
                    <CloudinaryUploadWidget
                      uwConfig={uwConfig}
                      setPublicId={setPublicId}
                      setUploadedUrl={(imageUrl) => {
                        handleCertUpload(imageUrl, "front");
                      }}
                      notList={true}
                    />
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">Back Certificate</div>
                  {realEstateTypes && (
                    <CloudinaryUploadWidget
                      uwConfig={uwConfig}
                      setPublicId={setPublicId}
                      setUploadedUrl={(imageUrl) => {
                        handleCertUpload(imageUrl, "back");
                      }}
                      notList={true}
                    />
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">
                    Ownership Certificate
                  </div>
                  {realEstateTypes && (
                    <CloudinaryUploadWidget
                      uwConfig={uwConfig}
                      setPublicId={setPublicId}
                      setUploadedUrl={(imageUrl) => {
                        handleCertUpload(imageUrl, "ownership");
                      }}
                      notList={true}
                    />
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">Registration Book</div>
                  {realEstateTypes && (
                    <CloudinaryUploadWidget
                      uwConfig={uwConfig}
                      setPublicId={setPublicId}
                      setUploadedUrl={(imageUrl) => {
                        handleCertUpload(imageUrl, "book");
                      }}
                      notList={true}
                    />
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">
                    Relationship Document
                  </div>
                  {realEstateTypes && (
                    <CloudinaryUploadWidget
                      uwConfig={uwConfig}
                      setPublicId={setPublicId}
                      setUploadedUrl={(imageUrl) => {
                        handleCertUpload(imageUrl, "docu");
                      }}
                      notList={true}
                    />
                  )}
                </div>
                <div>
                  <div className="text-center text-lg">
                    Authorization Contract
                  </div>
                  {realEstateTypes && (
                    <CloudinaryUploadWidget
                      uwConfig={uwConfig}
                      setPublicId={setPublicId}
                      setUploadedUrl={(imageUrl) => {
                        handleCertUpload(imageUrl, "contract");
                      }}
                      notList={true}
                    />
                  )}
                </div>
              </div>
              <div className="flex justify-center p-4">
                <button className="bg-mainBlue px-5 py-2 text-white rounded-lg hover:bg-darkerMainBlue">
                  Create
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default SellPage;
