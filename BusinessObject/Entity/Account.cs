namespace BusinessObject.Entity;

public class Account
{
    public int AccountId { get; set; }

    public string? Username { get; set; }
    public string AccountName { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string? AccountEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Citizen_identification { get; set; }
    public string? Address { get; set; }
    public Major? Major { get; set; }
    public int? MajorId { get; set; }
    public Role? Role { get; set; }
    public int RoleId { get; set; }
    public int Account_Status { get; set; }
    public DateTime Date_Created { get; set; } = DateTime.UtcNow;
    public DateTime Date_End { get; set; }
    public string? BankingNumber { get; set; }
    public string? BankingCode { get; set; } //ACB, NCB, AGR ...
    public string? FirebaseToken { get; set; }


    public List<RealEstate> RealEstate { get; set; }
    public List<Auction> Auctions { get; set; }
    public List<MoneyTransaction> MoneyTransactionsSent { get; set; }
    public List<MoneyTransaction> MoneyTransactionsReceived { get; set; }
    public List<News> NewsCreated { get; set; }
    public List<DepositAmount> DepositAmount { get; set; }
    public List<AuctionAccounting> WonAuctionAccountings { get; set; }
    public List<AuctionAccounting> OwnedAuctionAccountings { get; set; }
    public List<ParticipateAuctionHistory> ParticipateAuctionHistory { get; set; }
    public List<Notification> Notifications { get; set; }
}