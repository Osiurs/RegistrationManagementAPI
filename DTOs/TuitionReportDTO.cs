namespace RegistrationManagementAPI.DTOs
{
    public class TuitionReportDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public decimal TotalTuition { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
