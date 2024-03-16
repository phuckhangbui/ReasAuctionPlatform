namespace API.DTOs
{
    public class NewsCreate
    {
        public string ThumbnailUri { get; set; }
        public string NewsTitle { get; set; }
        public string NewsSumary { get; set; }
        public string NewsContent { get; set; }
    }
}
