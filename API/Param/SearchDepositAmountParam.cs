using API.Helper;

namespace API.Param
{
    public class SearchDepositAmountParam : PaginationParams
    {
        public int AmountFrom { get; set; }
        public int AmountTo { get; set; }
        public DateTime DepositDateFrom { get; set; }
        public DateTime DepositDateTo { get; set; }
    }
}
