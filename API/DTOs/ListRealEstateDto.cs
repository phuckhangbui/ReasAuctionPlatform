using API.Entity;

namespace API.DTOs
{
    public class ListRealEstateDto
    {
        public int ReasId { get; set; }
        public string ReasName { get; set; }
        public string ReasPrice { get; set; }
        public string ReasTypeName { get; set; }
        public int ReasStatus { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
