namespace Repository.DTOs
{
    public class VnPayRefundUrlDto
    {
        public string RequestId { get; set; }
        public string Command { get; set; } = "refund";
        public string TransactionType { get; set; } = "02";
        public long Amount { get; set; }
        public string TxnRef { get; set; }
        public string OrderInfo { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string IpAddr { get; set; }
        public string SecureHash { get; set; }

        public string Version { get; set; } // Alphanumeric[1,8]
        public string TmnCode { get; set; } // Alphanumeric[8]
        public string HashSecret { get; set; } // Alphanumeric[32,256]
        public string API { get; set; }

    }
}
