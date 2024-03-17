namespace Repository.DTOs
{
    public class MoneyTransactionDto
    {
        public int TransactionId { get; set; }
        public int TransactionStatus { get; set; }
        public string? TransactionNo { get; set; }
        public double Money { get; set; }
        public DateTime DateExecution { get; set; }
        public string? TransactionType { get; set; }
    }
}
