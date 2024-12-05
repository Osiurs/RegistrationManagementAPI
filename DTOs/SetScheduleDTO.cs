namespace RegistrationManagementAPI.DTOs
{
    public class SetScheduleDTO
    {
        public int ScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string Classroom { get; set; }
        public string RoomNumber { get; set; } // Thêm RoomNumber
    }
}
