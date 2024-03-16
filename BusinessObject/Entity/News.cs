namespace BusinessObject.Entity;

public class News
{
    public int NewsId { get; set; }
    public Account AccountCreate { get; set; }
    public int AccountCreateId { get; set; }
    public string Thumbnail { get; set; }
    public string AccountName { get; set; }
    public string NewsTitle { get; set; }
    public string NewsSumary { get; set; }
    public string NewsContent { get; set; }
    public DateTime DateCreated { get; set; }
}