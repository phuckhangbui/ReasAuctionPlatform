namespace Repository.DTOs
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int NotificationType { get; set; }
        public int AccountReceiveId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
