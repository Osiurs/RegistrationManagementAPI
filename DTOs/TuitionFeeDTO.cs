namespace RegistrationManagementAPI.DTOs
{
    public class TuitionFeeDTO
    {
        public int TuitionId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public decimal TotalTuition { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Status { get; set; }
        public DateTime? PaymentDeadline { get; set; }
        public string Semester { get; set; }
    }
}
