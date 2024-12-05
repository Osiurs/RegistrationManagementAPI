namespace RegistrationManagementAPI.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; } // UserID của người gửi
        public int ReceiverId { get; set; } // UserID của người nhận
        public string Content { get; set; }
        public DateTime SentDate { get; set; }

        // Liên kết với User
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
