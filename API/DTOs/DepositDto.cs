namespace API.DTOs
{
    public class DepositDto
    {
        public int reasId { get; set; }
        public string? reasName { get; set; }
        public DateTime? dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public int status { get; set; }
    }
}
