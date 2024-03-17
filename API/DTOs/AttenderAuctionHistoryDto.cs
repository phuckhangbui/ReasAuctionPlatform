namespace API.DTOs
{
    public class AttenderAuctionHistoryDto
    {
        public int AuctionId { get; set; }
        public int ReasId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public double LastBid { get; set; }
        public int DepositStatus { get; set; }
    }
}
