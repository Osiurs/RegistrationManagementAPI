namespace RegistrationManagementAPI.Entities
{
    public class TuitionFee
    {
        public int TuitionId { get; set; } // ID duy nhất của học phí


        // Thông tin về học phí
        public decimal Amount { get; set; } // Tổng học phí
        public decimal PaidAmount { get; set; } = 0; // Số tiền đã thanh toán
        public string Semester { get; set; } // Học kỳ (VD: "2024-1")
        public DateTime? PaymentDeadline { get; set; } // Hạn thanh toán (có thể null)

        // Trạng thái
        public string? Status { get; set; } = "Pending"; // Trạng thái (Pending, Paid, Overdue, etc.)

        // Metadata
        public DateTime? CreatedAt { get; set; } = DateTime.Now; // Thời gian tạo
        public DateTime? UpdatedAt { get; set; } = DateTime.Now; // Thời gian cập nhật
        // Quan hệ với Student
        public int StudentId { get; set; }
        public Student? Student { get; set; } // Nullable navigation property


    }
}
