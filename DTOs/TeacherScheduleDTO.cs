namespace RegistrationManagementAPI.DTOs
{
    public class TeacherScheduleDTO
    {
        public int ScheduleId { get; set; }
        public string CourseName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string RoomNumber { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
