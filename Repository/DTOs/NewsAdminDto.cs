namespace Repository.DTOs
{
    public class NewsAdminDto
    {
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsSumary { get; set; }
        public string Thumbnail { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
