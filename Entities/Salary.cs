namespace RegistrationManagementAPI.Entities
{
    public class Salary
    {
        public int SalaryId { get; set; } // ID duy nhất của khoản lương

        // Quan hệ với Teacher
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } // Navigation property đến giáo viên

        // Thông tin về lương
        public decimal Amount { get; set; } // Tổng lương
        public decimal PaidAmount { get; set; } = 0; // Số tiền đã thanh toán
        // Trạng thái
        public string Status { get; set; } = "Pending"; // Trạng thái (Pending, Paid, Overdue, etc.)

        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Thời gian tạo
        public DateTime? UpdatedAt { get; set; } = DateTime.Now; // Thời gian cập nhật
    }
}
