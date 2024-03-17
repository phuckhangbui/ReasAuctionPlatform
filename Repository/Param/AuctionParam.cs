using Repository.Paging;

namespace Repository.Param
{
    public class AuctionParam : PaginationParams
    {
        public string? Keyword { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }
}
