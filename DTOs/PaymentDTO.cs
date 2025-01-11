namespace RegistrationManagementAPI.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } // Kết hợp FirstName + LastName
        public string StudentEmail { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationStatus { get; set; }
        public int CourseId { get; set; }
    }

}
