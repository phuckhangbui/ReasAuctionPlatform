using Repository.Paging;
namespace Repository.Param
{
    public class MemberMoneyTransactionParam : PaginationParams
    {
        public string? DateExecutionFrom { get; set; }
        public string? DateExecutionTo { get; set; }
    }
}
