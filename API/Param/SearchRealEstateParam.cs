using API.Helper;

namespace API.Param
{
    public class SearchRealEstateParam : PaginationParams
    {
        public string? ReasName { get; set; }
        public int? ReasPriceFrom { get; set; }
        public int? ReasPriceTo { get; set; }
        public int ReasStatus { get; set; }
    }
}
