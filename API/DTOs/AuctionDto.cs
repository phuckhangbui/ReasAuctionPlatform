namespace API.DTOs
{
    public class AuctionDto
    {
        public int AuctionId { get; set; }
        public int ReasId { get; set; }
        public string ReasName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public double FloorBid { get; set; }
        public string Status { get; set; }
    }
}
