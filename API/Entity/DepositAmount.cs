namespace API.Entity;

public class DepositAmount
{
    public int DepositId { get; set; }
    public Rule Rule { get; set; }
    public int RuleId { get; set; }
    public Account AccountSign { get; set; }
    public int AccountSignId { get; set; }
    public RealEstate RealEstate { get; set; }
    public int ReasId { get; set; }
    public double Amount { get; set; }
    public DateTime? DepositDate { get; set; }
    public DateTime CreateDepositDate { get; set; }
    public int Status { get; set; }
}