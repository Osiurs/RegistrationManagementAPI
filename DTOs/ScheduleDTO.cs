namespace RegistrationManagementAPI.DTOs
{
    public class ScheduleDTO
    {
        public TimeSpan StartTime { get; set; } // Định dạng TIME
        public TimeSpan EndTime { get; set; }   // Định dạng TIME
        public string TeacherName { get; set; }
        public string CourseName { get; set; }
    }

}
