import { createContext, useEffect, useState, FC } from "react";

// Define types for Cloudinary configuration and the setPublicId function
type CloudinaryConfig = {
  cloudName: string;
  uploadPreset: string;
  folder: string;
};

type SetPublicIdFunction = (publicId: string) => void;

// Define the context type
type CloudinaryScriptContextType = {
  loaded: boolean;
};

// Create a context to manage the script loading state
const CloudinaryScriptContext = createContext<CloudinaryScriptContextType>({
  loaded: false,
});

interface CloudinaryUploadWidgetProps {
  uwConfig: CloudinaryConfig;
  setPublicId: SetPublicIdFunction;
  setUploadedUrl: (url: string) => void; // Thêm prop để truyền đường dẫn ảnh đã upload đi
  notList: boolean;
}

const CloudinaryUploadWidget: FC<CloudinaryUploadWidgetProps> = ({
  uwConfig,
  setPublicId,
  setUploadedUrl,
  notList,
}) => {
  const [loaded, setLoaded] = useState(false);
  const [backgroundImage, setBackgroundImage] = useState<string>("");

  useEffect(() => {
    const scriptId = "cloudinary-upload-widget-script";

    const loadScript = () => {
      const existingScript = document.getElementById(scriptId);

      if (!existingScript) {
        const script = document.createElement("script");
        script.src = "https://upload-widget.cloudinary.com/global/all.js";
        script.id = scriptId;
        script.onload = () => {
          setLoaded(true);
        };
        document.body.appendChild(script);
      } else {
        setLoaded(true);
      }
    };

    loadScript();

    return () => {
      // Clean up script tag
      const script = document.getElementById(scriptId);
      if (script) {
        document.body.removeChild(script);
      }
    };
  }, []);

  const initializeCloudinary = () => {
    if ((window as any).cloudinary) {
      const myWidget = (window as any).cloudinary.createUploadWidget(
        uwConfig,
        (error: any, result: any) => {
          if (!error && result && result.event === "success") {
            console.log("Done! Here is the image info: ", result.info);
            setPublicId(result.info.public_id);
            setUploadedUrl(result.info.secure_url);
            setBackgroundImage(result.info.secure_url); // Set the uploaded image as the background
          }
        }
      );
      myWidget.open();
    } else {
      console.error("Cloudinary script is not loaded.");
    }
  };

  return (
    <CloudinaryScriptContext.Provider value={{ loaded }}>
      {loaded && (
        <div
          className="flex items-center justify-center w-full h-64"
          onClick={() => {
            initializeCloudinary();
          }}
          id="upload_widget"
        >
          {backgroundImage && notList === true ? (
            <img
              className="text-transparent w-full h-full object-fill rounded-lg"
              src={backgroundImage}
            />
          ) : (
            <label
              htmlFor="dropzone-file"
              className="flex flex-col items-center justify-center w-full h-full border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 hover:bg-gray-100 "
            >
              <div className="flex flex-col items-center justify-center pt-5 pb-6">
                <p className="mb-2 text-sm text-gray-500 ">
                  <span className="font-semibold">Click to upload</span>
                </p>
                <p className="text-xs text-gray-500 ">SVG, PNG or JPG</p>
              </div>
            </label>
          )}
        </div>
      )}
    </CloudinaryScriptContext.Provider>
  );
};

export default CloudinaryUploadWidget;
export type { CloudinaryConfig, SetPublicIdFunction };
export { CloudinaryScriptContext };
