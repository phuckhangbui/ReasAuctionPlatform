namespace Repository.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string AccountName { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public bool isNewAccount { get; set; }
    }
}
