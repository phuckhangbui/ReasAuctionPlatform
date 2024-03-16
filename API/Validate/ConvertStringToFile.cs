//namespace API.Validate
//{
//    public class ConvertStringToFile
//    {
//        public IFormFile ConvertToIFormFile(string filePath)
//        {
//            byte[] fileBytes;
//            using (var stream = new FileStream(filePath, FileMode.Open))
//            {
//                using (var memoryStream = new MemoryStream())
//                {
//                    stream.CopyTo(memoryStream);
//                    fileBytes = memoryStream.ToArray();
//                }
//            }

//            var formFile = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, null, Path.GetFileName(filePath));
//            return formFile;
//        }
//    }
//}
