namespace BusinessObject.Entity
{
    public class ParticipateAuctionHistory
    {
        public int ParticipateAuctionHistoryId { get; set; }
        public int AuctionAccountingId { get; set; }
        public AuctionAccounting? AuctionAccounting { get; set; }
        public int AccountBidId { get; set; }
        public Account? AccountBid { get; set; }
        public double LastBid { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
    }
}
