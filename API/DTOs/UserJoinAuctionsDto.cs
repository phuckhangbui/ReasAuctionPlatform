namespace API.DTOs
{
    public class UserJoinAuctionsDto
    {
        public int accountId {  get; set; }
        public string accountName { get; set; }
        public string accountEmail { get; set; }
        public int numberOfAuction { get; set; }
    }
}
