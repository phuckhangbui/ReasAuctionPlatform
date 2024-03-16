namespace Service.VnPay
{
    public class VnPayProperties
    {
        public string Version { get; set; } // Alphanumeric[1,8]
        public string TmnCode { get; set; } // Alphanumeric[8]
        public string HashSecret { get; set; } // Alphanumeric[32,256]
        public string Url { get; set; } // Alphanumeric[32,256]
        public string API { get; set; }
    }
}
