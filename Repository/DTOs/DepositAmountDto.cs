namespace Repository.DTOs
{
    public class DepositAmountDto
    {
        public int DepositId { get; set; }
        public int RuleId { get; set; }
        public int AccountSignId { get; set; }
        public int ReasId { get; set; }
        public double Amount { get; set; }
        public DateTime DepositDate { get; set; }
        public DateTime CreateDepositDate { get; set; }
        public int Status { get; set; }
        public string DisplayStatus { get; set; }
    }
}
