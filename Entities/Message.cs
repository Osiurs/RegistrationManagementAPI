namespace RegistrationManagementAPI.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int? CourseId { get; set; }  // For group messages
        public string Content { get; set; }
        public DateTime SentDate { get; set; }

        // Navigation properties
        public User Sender { get; set; }
        public Course Course { get; set; } // Nullable
    }

}
