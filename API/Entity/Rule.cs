namespace API.Entity;

public class Rule
{
    public int RuleId { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}