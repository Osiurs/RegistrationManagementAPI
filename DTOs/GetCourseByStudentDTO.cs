namespace RegistrationManagementAPI.DTOs
{
    public class GetCourseByStudentDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewCount { get; set; }
        public string ImageUrl { get; set; }
        public string TeacherName { get; set; }
        public string? DayOfWeek { get; set; } // Ngày học trong tuần (ví dụ: "Monday, Wednesday, Friday")
        public TimeSpan? StartTime { get; set; } // Giờ bắt đầu
        public TimeSpan? EndTime { get; set; } // Giờ kết thúc
        public string RoomNumber { get; set; }
    }

}
