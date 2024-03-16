using Repository.Paging;

namespace Repository.Param
{
    public class SearchNewsParam : PaginationParams
    {
        public string? KeyWord { get; set; }
    }
}
