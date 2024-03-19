namespace Repository.DTOs
{
    public class AuctionNotCancelDto
    {
        public int AuctionId { get; set; }
        public int ReasId { get; set; }
        public string? ReasName { get; set; }
        public string? DateStart { get; set; }
        public string? DateEnd { get; set; }
        public double FloorBid { get; set; }
        public int Status { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
