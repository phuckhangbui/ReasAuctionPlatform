namespace Repository.DTOs
{
    public class MoneyTransactionDetailDto : MoneyTransactionDto
    {
        public int? AccountSendId { get; set; }
        public string? AccountSendName { get; set; }
        public int? AccountReceiveId { get; set; }
        public string? AccountReceiveName { get; set; }
        public int? ReasId { get; set; }
        public string? ReasName { get; set; }
        public int? DepositId { get; set; }
        public string? TxnRef { get; set; }
        public string? AccountSendBankingNumber { get; set; }
        public string? AccountSendBankingCode { get; set; }
        public string? AccountReceiveBankingNumber { get; set; }
        public string? AccountReceiveBankingCode { get; set; }
    }
}
