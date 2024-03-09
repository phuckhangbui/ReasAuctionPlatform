namespace API.DTOs
{
    public class DepositDto
    {
        public int DepositId { get; set; }
        public int ReasId { get; set; }
        public string? ReasName { get; set; }
        public int AccountSignId { get; set; }
        public string? AccountSignName { get; set; }
        public double Amount { get; set; }
        public DateTime? DepositDate { get; set; }
        public DateTime CreateDepositDate { get; set; }
        public int Status { get; set; }
    }
}
