namespace BusinessObject.Entity;

public class RealEstate
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
    public string Message { get; set; }
    public int Type_Reas { get; set; }
    public Type_REAS Type_REAS { get; set; }
    public Account AccountOwner { get; set; }
    public int AccountOwnerId { get; set; }
    public string AccountOwnerName { get; set; }
    public DateTime DateCreated { get; set; }

    public List<RealEstatePhoto> Photos { get; set; }

    public RealEstateDetail Detail { get; set; }
}