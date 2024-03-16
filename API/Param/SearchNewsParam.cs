using API.Helper;

namespace API.Param
{
    public class SearchNewsParam : PaginationParams
    {
        public string? KeyWord { get; set; }
    }
}
