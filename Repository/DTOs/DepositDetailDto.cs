namespace Repository.DTOs
{
    public class DepositDetailDto : DepositDto
    {
        public int RuleId { get; set; }
        public int AccountSignId { get; set; }
        public int ReasId { get; set; }
    }
}
