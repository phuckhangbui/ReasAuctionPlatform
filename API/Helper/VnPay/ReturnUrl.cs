using API.Entity;
using API.Param.Enums;

namespace API.Helper.VnPay
{
    public class ReturnUrl
    {
        public static MoneyTransaction ProcessReturnUrlForDepositAuction(Dictionary<string, string> vnpayData, string vnp_HashSecret)
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
            String vnp_SecureHash = vnpayData["vnp_SecureHash"];
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    //Thanh toan thanh cong
                    transaction.TransactionStatus = (int)TransactionStatus.success;
                    transaction.TransactionNo = vnpay.GetResponseData("vnp_TransactionNo");
                    transaction.TxnRef = vnpay.GetResponseData("vnp_TxnRef");
                    transaction.TypeId = (int)TransactionType.Deposit_Auction_Fee;
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
