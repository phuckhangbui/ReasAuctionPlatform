using Repository.Paging;

namespace Repository.Param
{
    public class AuctionHistoryParam : PaginationParams
    {
        public int AccountId { get; set; }
    }
}
