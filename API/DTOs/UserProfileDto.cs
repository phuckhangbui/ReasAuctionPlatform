namespace API.DTOs
{
    public class UserProfileDto
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CitizenIdentification { get; set; }
        public string? Address { get; set; }
        public int? MajorId { get; set; }
        public string MajorName { get; set; }
        public string? BankingCode { get; set; }
        public string? BankingNumber { get; set; }
        public Dictionary<int, string>? Major { get; set; }

    }

    public class UserUpdateProfileInfo
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CitizenIdentification { get; set; }
        public string? Address { get; set; }
        public int? MajorId { get; set; }
        public string? BankingCode { get; set; }
        public string? BankingNumber { get; set; }
    }
}
