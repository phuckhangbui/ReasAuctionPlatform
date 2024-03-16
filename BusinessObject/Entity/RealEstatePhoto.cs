namespace BusinessObject.Entity;

public class RealEstatePhoto
{
    public int ReasPhotoId { get; set; }
    public string ReasPhotoUrl { get; set; }
    public RealEstate RealEstate { get; set; }
    public int ReasId { get; set; }
}