namespace Repository.DTOs
{
    public class RealEstateInfoDto
    {
        public int ReasDetailId { get; set; }
        public int ReasId { get; set; }
        public string Reas_Cert_Of_Land_Img_Front { get; set; }
        public string Reas_Cert_Of_Land_Img_After { get; set; }
        public string Reas_Cert_Of_Home_Ownership { get; set; }
        public string Reas_Registration_Book { get; set; }
        public string Documents_Proving_Marital_Relationship { get; set; }
        public string Sales_Authorization_Contract { get; set; }
        public List<RealEstatePhotoDto> Photos { get; set; }
    }
}
