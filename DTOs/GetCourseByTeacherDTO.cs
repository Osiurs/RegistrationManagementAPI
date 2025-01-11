namespace RegistrationManagementAPI.DTOs
{
    public class GetCourseByTeacherDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewCount { get; set; }
        public string ImageUrl { get; set; }
        public string TeacherName { get; set; }
        public string DayOfWeek { get; set; } // Các ngày trong tuần
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string RoomNumber { get; set; }
    }
}
