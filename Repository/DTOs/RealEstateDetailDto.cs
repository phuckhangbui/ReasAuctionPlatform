namespace Repository.DTOs
{
    public class RealEstateDetailDto
    {
        public int ReasId { get; set; }
        public string ReasName { get; set; }
        public string ReasAddress { get; set; }
        public double ReasPrice { get; set; }
        public int ReasArea { get; set; }
        public string ReasDescription { get; set; }
        public int ReasStatus { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int AccountOwnerId { get; set; }
        public string AccountOwnerName { get; set; }
        public string Type_REAS_Name { get; set; }
        public DateTime DateCreated { get; set; }

        public List<RealEstatePhotoDto> Photos { get; set; }
        public RealEstatePaper Detail { get; set; }
    }
}
