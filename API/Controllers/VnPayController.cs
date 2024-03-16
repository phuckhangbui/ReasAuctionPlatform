using API.Helper.VnPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [ApiController]
    public class VnPayController : ControllerBase
    {

        private readonly VnPayProperties _vnPayProperties;


        public VnPayController(IOptions<VnPayProperties> vnPayProperties)
        {
            _vnPayProperties = vnPayProperties.Value;
        }

        [HttpGet("IPN")]
        public ActionResult<IpnResponseDto> ProcessIPN([FromQuery] Dictionary<string, string> vnpayData)
        {
            string vnp_HashSecret = _vnPayProperties.HashSecret;

            // Call the ProcessIPN function from your service
            IpnResponseDto response = IPN.ProcessIPN(vnpayData, vnp_HashSecret);

            if (response.RspCode.Equals("00"))
            {
                // handle success logic here
            }

            // Return the response
            return Ok(response);
        }

        //[HttpGet("Refund")]
        //public IActionResult RefundTransaction(string requestId, string transactionNo, string TnxRef, string transactionDate)
        //{
        //    try
        //    {
        //        VnPayRefundUrlDto refundData = new VnPayRefundUrlDto();
        //        refundData.API = _vnPayProperties.API;
        //        refundData.TmnCode = _vnPayProperties.TmnCode;
        //        refundData.Version = _vnPayProperties.Version;
        //        refundData.HashSecret = _vnPayProperties.HashSecret;

        //        refundData.RequestId = requestId;
        //        refundData.TxnRef = TnxRef;
        //        refundData.Amount = 100000;
        //        refundData.TransactionNo = transactionNo;
        //        refundData.TransactionDate = transactionDate;
        //        refundData.CreateBy = "khag";
        //        refundData.CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
        //        refundData.IpAddr = "test";
        //        refundData.OrderInfo = "test refund vnpay";

        //        // Construct the data for signing
        //        var signData = $"{refundData.RequestId}|{refundData.Version}|{refundData.Command}|{refundData.TmnCode}|{refundData.TransactionType}|{refundData.TxnRef}|{refundData.Amount}|{refundData.TransactionNo}|{refundData.TransactionDate}|{refundData.CreateBy}|{refundData.CreateDate}|{refundData.IpAddr}|{refundData.OrderInfo}";

        //        // Generate the secure hash
        //        var vnp_SecureHash = Utils.HmacSHA512(refundData.HashSecret, signData);

        //        // Add the secure hash to the refund data
        //        refundData.SecureHash = vnp_SecureHash;

        //        // Convert the refund data to JSON
        //        var jsonData = JsonSerializer.Serialize(refundData);

        //        // Create and send the HTTP request
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(refundData.API);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            streamWriter.Write(jsonData);
        //        }

        //        // Get the HTTP response
        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        string strData;

        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            strData = streamReader.ReadToEnd();
        //        }

        //        // Return the response
        //        return Ok(strData);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}
    }
}
