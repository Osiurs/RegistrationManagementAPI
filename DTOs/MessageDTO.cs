namespace RegistrationManagementAPI.DTOs
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public int? CourseId { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
    }
}