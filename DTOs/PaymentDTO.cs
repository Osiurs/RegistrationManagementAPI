namespace RegistrationManagementAPI.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } // "Cash", "Card", "Online"
        public int StudentId { get; set; }
        public int RegistrationId { get; set; }
    }
}