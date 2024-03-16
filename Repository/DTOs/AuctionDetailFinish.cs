namespace Repository.DTOs
{
    public class AuctionDetailFinish
    {
        public int AuctionId { get; set; }
        public int ReasId { get; set; }
        public string ReasName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public double FloorBid { get; set; }
        public string Status { get; set; }
        public int AccountCreateId { get; set; }
        public int AccountOwnerId { get; set; }
        public int AccountWinnerId { get; set; }
        public string AccountCreateName { get; set; }
        public string AccountOwnerName { get; set; }
        public string AccountOwnerEmail { get; set; }
        public string AccountOwnerPhone { get; set; }
        public string AccountWinnerName { get; set; }
        public string AccountWinnerEmail { get; set; }
        public string AccountWinnerPhone { get; set; }
        public double FinalAmount { get; set; }
        public double DepositAmout { get; set; }
        public double CommisionAmount { get; set; }
        public double OwnerReceiveAmount { get; set; }

    }
}
