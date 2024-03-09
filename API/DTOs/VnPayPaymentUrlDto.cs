namespace API.DTOs
{
    public class VnPayPaymentUrlDto
    {
        public long Amount { get; set; } // Numeric[1,12]
        public DateTime CreateDate { get; set; } // Numeric[14]
        public DateTime ExpireDate { get; set; } // Numeric[14]
        public string CurrCode { get; set; } = "VND";// Alpha[3]
        public string Locale { get; set; } // Alpha[2,5]
        public string OrderInfo { get; set; } // Alphanumeric[1,255]
        public string ReturnUrl { get; set; } // Alphanumeric[10,255]
        public string TxnRef { get; set; } // Alphanumeric[1,100]

        public string Version { get; set; } // Alphanumeric[1,8]
        public string TmnCode { get; set; } // Alphanumeric[8]
        public string HashSecret { get; set; } // Alphanumeric[32,256]
        public string Url { get; set; } // Alphanumeric[32,256]

    }
}
