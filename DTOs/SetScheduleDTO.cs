namespace RegistrationManagementAPI.DTOs
{
    public class SetScheduleDTO
{
    public int ScheduleId { get; set; }
    public TimeSpan StartTime { get; set; } // Định dạng TIME
    public TimeSpan EndTime { get; set; }   // Định dạng TIME
    public string CourseName { get; set; }
    public string TeacherName { get; set; }
    public string Classroom { get; set; }
    public string RoomNumber { get; set; }
    public DateTime ScheduleDate { get; set; }
}

}
