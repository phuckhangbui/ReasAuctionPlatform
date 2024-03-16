namespace API.DTOs
{
    public class AuctionDetailOnGoing
    {
        public int AuctionId { get; set; }
        public int ReasId { get; set; }
        public string ReasName { get; set; }
        public DateTime DateStart { get; set; }
        public double FloorBid { get; set; }
        public string Status { get; set; }
        public int AccountCreateId { get; set; }
        public int AccountOwnerId { get; set; }
        public string AccountCreateName { get; set; }
        public string AccountOwnerName { get; set; }
        public string AccountOwnerEmail { get; set;}
        public string AccountOwnerPhone { get; set;}
    }
}
