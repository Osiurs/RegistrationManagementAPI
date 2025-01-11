namespace RegistrationManagementAPI.DTOs
{
    public class SetScheduleRequestDTO
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public int ClassroomId { get; set; }
        public int StudentId {get; set;}
        public string DayOfWeek { get; set; }
    }
}
