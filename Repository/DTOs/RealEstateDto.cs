namespace Repository.DTOs
{
    public class RealEstateDto
    {
        public int ReasId { get; set; }
        public string ReasName { get; set; }
        public double ReasPrice { get; set; }
        public int ReasArea { get; set; }
        public string ReasTypeName { get; set; }
        public string ReasStatus { get; set; }
        public bool Flag { get; set; }
        public string UriPhotoFirst { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
