namespace RegistrationManagementAPI.DTOs
{
    public class MessageReadStatusDTO
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
    }

}
