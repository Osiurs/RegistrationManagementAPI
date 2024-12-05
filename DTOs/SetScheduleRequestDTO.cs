namespace RegistrationManagementAPI.DTOs
{
    public class SetScheduleRequestDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public int ClassroomId { get; set; }
        public int StudentId {get; set;}
    }
}
