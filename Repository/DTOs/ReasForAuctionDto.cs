namespace Repository.DTOs
{
    public class ReasForAuctionDto
    {
        public int reasId { get; set; }
        public string reasName { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public int numberOfUser { get; set; }
    }
}
