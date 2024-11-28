namespace RegistrationManagementAPI.DTOs
{
    public class SalaryReportDTO
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
