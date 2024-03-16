import { MagnifyingGlassIcon } from "@heroicons/react/20/solid";
import { Input} from "@material-tailwind/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import CloudinaryUploadWidget, {
  CloudinaryConfig,
  SetPublicIdFunction,
} from "../../../../Config/cloudinary";
import { Cloudinary } from "@cloudinary/url-gen";
import { Table, TableProps, Descriptions, Button, notification } from "antd";
import { useState, useEffect } from "react";
import {
  getNewsAdmin,
  getNewsAdminById,
  searchNewsAdmin,
  updateNews,
} from "../../../../api/adminnews";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

const AdminNewsList: React.FC = () => {
  const [search, setSearch] = useState<searchNewsAdmin>({ KeyWord: "" });
  const [newsData, setNewsData] = useState<news[]>();
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [uploadedImage, setUploadedImage] = useState<string>("");
  const [updatedTitle, setUpdatedTitle] = useState<string>("");
  const [updatedSummary, setUpdatedSummary] = useState<string>("");
  const [updatedContent, setUpdatedContent] = useState<string>("");
  const [newsDetailData, setNewsDetailData] = useState<newsDetail>();
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const [newsID, setNewsId] = useState<Number | undefined>();
  const [initialTitle, setInitialTitle] = useState<string>(""); // State để lưu trữ giá trị ban đầu của title
  const [initialSummary, setInitialSummary] = useState<string>(""); // State để lưu trữ giá trị ban đầu của summary
  const [initialContent, setInitialContent] = useState<string>("");
  const [initialThumbnail, setInitialThumbnail] = useState<string>("");
  const { token } = useContext(UserContext);

  const [publicId, setPublicId] = useState<string>("");
  const [cloudName] = useState<string>("dqpsvl3nu");
  const [uploadPreset] = useState<string>("i0yxovxe");

  const [uwConfig] = useState<CloudinaryConfig>({
    cloudName,
    uploadPreset,
    // cropping: true,
    // showAdvancedOptions: true,
    // sources: [ "local", "url"],
    // multiple: false,
    folder: "News",
    // tags: ["users", "profile"],
    // context: {alt: "user_uploaded"},
    // clientAllowedFormats: ["images"],
    // maxImageFileSize: 2000000,
    // maxImageWidth: 2000,
    // theme: "purple",
  });

  const cld = new Cloudinary({
    cloud: {
      cloudName,
    },
  });

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

  const fetchUpdateNews = async (newsUpdateData: newsUpdate) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await updateNews(newsUpdateData, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching update news:", error);
    }
  };

  useEffect(() => {
    if (newsDetailData) {
      setInitialTitle(newsDetailData.newsTitle || "");
      setInitialSummary(newsDetailData.newsSumary || "");
      setInitialContent(newsDetailData.newsContent || "");
      setInitialThumbnail(newsDetailData.thumbnail || "");
    }
  }, [newsDetailData]);

  const fetchNewsDetail = async (
    newsId: Number | undefined,
    token: string | undefined
  ) => {
    try {
      if (token) {
        let data: newsDetail | undefined;
        data = await getNewsAdminById(newsId, token);
        setNewsDetailData(data);
        setNewsId(newsId);
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching news detail:", error);
    }
  };

  const fetchNewsList = async () => {
    try {
      if (token) {
        let data: news[] | undefined;
        if (search.KeyWord !== "") {
          data = await searchNewsAdmin(search, token);
        } else {
          data = await getNewsAdmin(token);
        }
        setNewsData(data);
      }
    } catch (error) {
      console.error("Error fetching member list:", error);
    }
  };

  useEffect(() => {
    fetchNewsList();
  }, [search, token]);

  const viewDetail = (NewsId: Number) => {
    fetchNewsDetail(NewsId, token);
  };

  const columns: TableProps<news>["columns"] = [
    {
      title: "Thumbnail",
      dataIndex: "thumbnail",
      width: "20%",
      render: (url: string) => {
        return (
          <img
            src={url}
            alt="Photo"
            style={{ maxWidth: "100%", maxHeight: "200px" }}
          />
        );
      },
    },
    {
      title: "Title",
      dataIndex: "newsTitle",
      width: "15%",
    },
    {
      title: "Sumary",
      dataIndex: "newsSumary",
      width: "20%",
    },
    {
      title: "Date Created",
      dataIndex: "dateCreated",
      width: "18%",
      render: (date_Created: Date) => formatDate(date_Created),
    },
    {
      title: "Actions",
      dataIndex: "operation",
      render: (_: any, news: news) => (
        <a onClick={() => viewDetail(news.newsId)}>View details</a>
      ),
      width: "15%",
    },
  ];

  const renderBorderedItems = () => {
    const items = [
      {
        key: "1",
        label: "Thumbnail",
        children: newsDetailData?.thumbnail || "",
        span: 3,
        render: (url: string) => (
          <div>
            {uploadedImage ? (
              <img
                className="h-96 w-full object-cover object-center"
                src={uploadedImage}
                alt="Uploaded"
                style={{ maxWidth: "100%", maxHeight: "200px" }}
              />
            ) : (
              <div>
                <img
                  className="h-97 w-97 rounded-lg object-cover object-center shadow-xl shadow-blue-gray-900/50"
                  src={url}
                  alt="Photo"
                  //style={{ maxWidth: "100%", maxHeight: "200px" }}
                />
                {isEditing ? (
                  <CloudinaryUploadWidget
                    uwConfig={uwConfig}
                    setPublicId={setPublicId as SetPublicIdFunction}
                    setUploadedUrl={setUploadedImage}
                    notList
                  />
                ) : (
                  <div></div>
                )}
              </div>
            )}
          </div>
        ),
      },
      {
        key: "2",
        label: "Title",
        children: isEditing ? (
          <Input
            crossOrigin={undefined}
            defaultValue={initialTitle}
            onChange={(e) => setUpdatedTitle(e.target.value)}
          />
        ) : (
          initialTitle
        ),
        span: 3,
      },
      {
        key: "3",
        label: "Summary",
        children: isEditing ? (
          <Input
            crossOrigin={undefined}
            defaultValue={initialSummary}
            onChange={(e) => setUpdatedSummary(e.target.value)}
          />
        ) : (
          initialSummary
        ),
        span: 3,
      },
      {
        key: "4",
        label: "Date Create",
        children: newsDetailData ? formatDate(newsDetailData.dateCreated) : "",
        span: 3,
      },
      {
        key: "5",
        label: "Content",
        children: isEditing ? (
          <CKEditor
            editor={ClassicEditor}
            data={initialContent}
            onChange={(e, editor) => setUpdatedContent(editor.getData())}
          />
        ) : (
          <div dangerouslySetInnerHTML={{ __html: initialContent }}></div>
        ),
        span: 3,
      },
    ];
    return items.map((item) => (
      <Descriptions.Item key={item.key} label={item.label} span={item.span}>
        {item.render ? item.render(item.children) : item.children}
      </Descriptions.Item>
    ));
  };

  const EditNews = () => {
    setIsEditing(true);
    // setUpdatedNewsData({
    //   dateCreated: new Date(),
    //   newsContent: initialContent,
    //   newsSumary: initialSummary,
    //   newsTitle: initialTitle,
    //   thumbnail: initialThumbnail,
    //   newsId: newsID,
    // });

    fetchNewsDetail(newsID, token);
  };


  const handleUpdate = () => {
    setIsEditing(false);
    let newsContent1;
    if (updatedContent !== "") {
      newsContent1 = updatedContent;
    } else {
      newsContent1 = initialContent;
    }

    // Kiểm tra và gán giá trị cho newsSummary
    let newsSummary1;
    if (updatedSummary !== "") {
      newsSummary1 = updatedSummary;
    } else {
      newsSummary1 = initialSummary;
    }

    // Kiểm tra và gán giá trị cho newsTitle
    let newsTitle1;
    if (updatedTitle !== "") {
      newsTitle1 = updatedTitle;
    } else {
      newsTitle1 = initialTitle;
    }

    // Kiểm tra và gán giá trị cho thumbnail
    let thumbnail1;
    if (uploadedImage !== "") {
      thumbnail1 = uploadedImage;
    } else {
      thumbnail1 = initialThumbnail;
    }

    const updatedNewsData1: newsUpdate = {
      newsContent: newsContent1,
      newsSumary: newsSummary1,
      newsTitle: newsTitle1,
      dateCreated: new Date(),
      newsId: newsID,
      thumbnail: thumbnail1,
    };
    getMessage(updatedNewsData1);
  };

  const handleBackToList = () => {
    setShowDetail(false);
    setIsEditing(false);
    setUpdatedContent("");
    setUpdatedSummary("");
    setUpdatedTitle("");
    setUploadedImage("");
    fetchNewsList();
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

  const getMessage = async (updatedNewsData: newsUpdate) => {
    const response = await fetchUpdateNews(updatedNewsData);
    if (response !== undefined && response) {
      if (response.statusCode === "MSG03") {
        openNotificationWithIcon("success", response.message);
      } else {
        openNotificationWithIcon(
          "error",
          "Something went wrong when executing operation. Please try again!"
        );
      }
      fetchNewsDetail(newsID, token);
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
              <Button onClick={EditNews}>Edit</Button> // Thêm nút Edit khi không trong chế độ sửa
            )}
          </div>
          <br />
          <Descriptions bordered title="Detail of News">
            {renderBorderedItems()}
          </Descriptions>
        </div>
      ) : (
        <div>
          {/* Bảng danh sách */}
          <h4>
            <strong>List News</strong>
          </h4>
          <br />
          <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
            <div className="w-full md:w-72 flex flex-row justify-start">
              <Input
                label="Search by Title"
                icon={<MagnifyingGlassIcon className="h-5 w-5" />}
                crossOrigin={undefined}
                onKeyDown={(e) => {
                  if (e.key === "Enter") {
                    setSearch({ KeyWord: e.currentTarget.value });
                  }
                }}
              />
            </div>
          </div>
          <Table columns={columns} dataSource={newsData} bordered />
        </div>
      )}
    </>
  );
};

export default AdminNewsList;
