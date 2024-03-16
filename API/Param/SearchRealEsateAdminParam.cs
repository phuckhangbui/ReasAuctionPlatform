using System.Collections;

namespace API.Param
{
    public class SearchRealEsateAdminParam
    {
        public string? reasName { get; set; }
        public int? reasPriceFrom { get; set; }
        public int? reasPriceTo { get; set; }
        public IEnumerable<int> reasStatus { get; set; }
    }
}
