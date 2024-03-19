using BusinessObject.Entity;
using BusinessObject.Enum;

namespace Service.VnPay
{
    public class ReturnUrl
    {
        public static MoneyTransaction ProcessReturnUrl(Dictionary<string, string> vnpayData, string vnp_HashSecret, TransactionType transactionType)
        {
            MoneyTransaction transaction = new MoneyTransaction();
            VnPayLibrary vnpay = new VnPayLibrary();

            foreach (var kvp in vnpayData)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(kvp.Key) && kvp.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(kvp.Key, kvp.Value);
                }
            }

            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnpayData["vnp_SecureHash"];
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            //bool checkSignature = vnpay.ValidateSignature("c85ad2998d07545289cce3c8085f78174cfdfdc5cf6a218945254f0161cedb166c25b89e08006b6d7dc59879a12594ca3be283cd62eae2741eb0dbb695846ddd", "BGNVMWIUSMXPEGVRMMGXTGWSFUMOJZEU");

            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    //Thanh toan thanh cong
                    transaction.TransactionStatus = (int)TransactionStatus.success;
                    transaction.TransactionNo = vnpay.GetResponseData("vnp_TransactionNo");
                    transaction.TxnRef = vnpay.GetResponseData("vnp_TxnRef");
                    transaction.TypeId = (int)transactionType;
                    transaction.Money = Convert.ToDouble(vnpay.GetResponseData("vnp_Amount")) / 100;
                    transaction.DateExecution = Utils.ParseDateString(vnpay.GetResponseData("vnp_PayDate"));
                }
                else
                {
                    return null;

                }
            }
            return transaction;
        }


    }
}
