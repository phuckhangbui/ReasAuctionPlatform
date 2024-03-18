namespace Repository.DTOs
{
    public class AuctionResultDto
    {
        public class AuctionData
        {
            public AuctionDetail Auction { get; set; }
            public double CurrentBid { get; set; }
            public string LastBid { get; set; }
            public int Status { get; set; }
            public long StatusChangeTime { get; set; }
            public bool IsBidded { get; set; }
            public Dictionary<string, UserBid> Users { get; set; }

            public int GetAccountWinId()
            {
                var user = Users.FirstOrDefault(u => u.Value.CurrentUserBid == CurrentBid);
                return int.Parse(user.Value.UserId);
            }

            //public AuctionDetail AuctionDetail { get; set; }
            //public List<UserBid> AuctionHistory { get; set; }
        }

        public class AuctionDetail
        {
            public int AuctionId { get; set; }
            public DateTime DateEnd { get; set; }
            public DateTime DateStart { get; set; }
            public double FloorBid { get; set; }
            public int ReasId { get; set; }
            public string ReasName { get; set; }
            public string Status { get; set; }

            //public int AuctionId { get; set; }

            //public int AccountWinId { get; set; }

            //public double WinAmount { get; set; }
        }

        public class UserBid
        {
            public double CurrentUserBid { get; set; }
            public string UserId { get; set; }
        }
    }
}
