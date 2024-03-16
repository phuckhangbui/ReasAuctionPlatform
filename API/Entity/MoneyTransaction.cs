namespace API.Entity;

public class MoneyTransaction
{
    public int TransactionId { get; set; }
    public int TransactionStatus { get; set; }
    public string? TransactionNo { get; set; }
    public string TxnRef { get; set; }
    public MoneyTransactionType Type { get; set; }
    public int TypeId { get; set; }
    public Account? AccountSend { get; set; }
    public int? AccountSendId { get; set; }
    public Account? AccountReceive { get; set; }
    public int? AccountReceiveId { get; set; }
    public RealEstate RealEstate { get; set; }
    public int? ReasId { get; set; }
    public double Money { get; set; }
    public DateTime DateExecution { get; set; }
    public DepositAmount DepositAmount { get; set; }
    public int? DepositId { get; set; }


}