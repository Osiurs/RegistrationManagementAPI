namespace RegistrationManagementAPI.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } // "Cash", "Card", "Online"

        // Relationships
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int RegistrationId { get; set; }
        public Registration Registration { get; set; }
    }
}