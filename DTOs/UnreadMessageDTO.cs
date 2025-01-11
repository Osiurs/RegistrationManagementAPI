namespace RegistrationManagementAPI.DTOs
{
    public class UnreadMessageDTO
    {
        public int MessageId { get; set; }
        public int CourseId { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
    }
}
