namespace RegistrationManagementAPI.DTOs
{
    public class SalaryDTO
    {
        public int SalaryId { get; set; }
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? Specialization { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal PaidAmount { get; set; }
        public string? Status { get; set; }
    }
}
