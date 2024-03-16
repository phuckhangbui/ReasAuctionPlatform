namespace Repository.DTOs;

public class AccountStaffDto
{
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    public string Username { get; set; }
    public string AccountEmail { get; set; }
    public string Account_Status { get; set; }
    public DateTime Date_Created { get; set; }
    public DateTime Date_End { get; set; }
}