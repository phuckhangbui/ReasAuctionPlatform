namespace API.DTOs
{
    public class DepositAmountUserDto
    {
        public int depositID {  get; set; }
        public int accountSignId { get; set; }
        public string accountName { get; set; }
        public string accountEmail { get; set; }
        public string? accountPhone { get; set; }
        public int reasId { get; set; }
        public double amount { get; set; }
        public DateTime? depositDate { get; set; }
        public string status { get; set; }
    }
}
