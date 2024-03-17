namespace API.DTOs
{
    public class CreateUpdateTaskDto
    {
        public int AccountAssignedId { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskContent { get; set; }
        public int Status { get; set; }
        public string? TaskNotes { get; set; }
    }
}
