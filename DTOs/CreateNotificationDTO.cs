namespace TaskManagement.DTOs
{
    public class CreateNotificationDTO
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "TaskAssigned";
        public int? RelatedTaskId { get; set; }
    }
}
