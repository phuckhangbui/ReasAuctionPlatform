namespace Repository.DTOs
{
    public class SearchAccountDto
    {
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountEmail { get; set; }
        public int RoleId { get; set; }
    }
}
