namespace Repository.DTOs
{
    public class ParticipateAuctionFinalDto
    {
        public int idAccount { get; set; }
        public string accountName { get; set; }
        public string accountEmail { get; set; }
        public string accountPhone { get; set; }
        public double lastBid { get; set; }
        public int isWinner { get; set; }
        public string note { get; set; }
    }
}
