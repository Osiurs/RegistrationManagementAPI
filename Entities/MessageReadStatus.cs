namespace RegistrationManagementAPI.Entities
{
    public class MessageReadStatus
    {
        public int MessageReadStatusId { get; set; }
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; } = false;

        // Navigation properties
        public Message Message { get; set; }
        public User User { get; set; }
    }
}
