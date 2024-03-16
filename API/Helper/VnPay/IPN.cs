namespace API.Helper.VnPay
{
    public class IPN
    {
        public static IpnResponseDto ProcessIPN(Dictionary<string, string> vnpayData, string vnp_HashSecret)
        {
            IpnResponseDto ipnResponseDto = new IpnResponseDto();
            VnPayLibrary vnpay = new VnPayLibrary();

            foreach (var kvp in vnpayData)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(kvp.Key) && kvp.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(kvp.Key, kvp.Value);
                }
            }

            //Lay danh sach tham so tra ve tu VNPAY
            //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
            //vnp_TransactionNo: Ma GD tai he thong VNPAY
            //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
            //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

            long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            String vnp_SecureHash = vnpayData["vnp_SecureHash"];
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (checkSignature)
            {
                //Cap nhat ket qua GD
                //Yeu cau: Truy van vao CSDL cua  Merchant => lay ra duoc OrderInfo
                //Giả sử OrderInfo lấy ra được như giả lập bên dưới
                OrderInfo order = new OrderInfo();//get from DB
                //order.OrderId = orderId;
                order.Amount = 100000;
                //order.PaymentTranId = vnpayTranId;
                order.Status = "0"; //0: Cho thanh toan,1: da thanh toan,2: GD loi

                //Kiem tra tinh trang Order
                if (order != null)
                {
                    if (order.Amount == vnp_Amount)
                    {
                        if (order.Status == "0")
                        {
                            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                            {
                                //Thanh toan thanh cong
                                //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                                //order.Status = "1";
                            }
                            else
                            {
                                //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                //  displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                                //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                                //order.Status = "2";
                            }

                            //Thêm code Thực hiện cập nhật vào Database 
                            //Update Database
                            ipnResponseDto.RspCode = "00";
                            ipnResponseDto.Message = "Confirm Success";

                        }
                        else
                        {
                            ipnResponseDto.RspCode = "02";
                            ipnResponseDto.Message = "Order already confirmed";
                        }
                    }
                    else
                    {
                        ipnResponseDto.RspCode = "04";
                        ipnResponseDto.Message = "invalid amount";
                    }
                }
                else
                {
                    ipnResponseDto.RspCode = "01";
                    ipnResponseDto.Message = "transaction not found";
                }
            }
            else
            {
                //log.InfoFormat("Invalid signature, InputData={0}", vnp_SecureHash);
                ipnResponseDto.RspCode = "97";
                ipnResponseDto.Message = "Invalid signature";
            }

            return ipnResponseDto;
        }


        public class OrderInfo
        {
            public long OrderId { get; set; }
            public long Amount { get; set; }
            public string OrderDesc { get; set; }

            public DateTime CreatedDate { get; set; }
            public string Status { get; set; }

            public long PaymentTranId { get; set; }
            public string BankCode { get; set; }
            public string PayStatus { get; set; }


        }

    }

}