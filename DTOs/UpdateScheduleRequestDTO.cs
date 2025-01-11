namespace RegistrationManagementAPI.DTOs
{
    public class UpdateScheduleRequestDTO
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int CourseId { get; set; }
        public int ClassroomId { get; set; }
        public string DayOfWeek { get; set; } // Can be a single day or multiple days (comma-separated)
    }

}
