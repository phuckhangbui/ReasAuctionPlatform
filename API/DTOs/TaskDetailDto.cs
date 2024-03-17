namespace API.DTOs
{
    public class TaskDetailDto : TaskDto
    {
        public string? TaskNotes { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskContent { get; set; }
    }
}
