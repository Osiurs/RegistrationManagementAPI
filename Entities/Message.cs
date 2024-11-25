namespace RegistrationManagementAPI.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public int StudentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }

        // Navigation property
        public Student Student { get; set; }
    }
}
