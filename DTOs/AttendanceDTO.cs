namespace RegistrationManagementAPI.DTOs
{
    public class AttendanceDTO
    {
        public int AttendanceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public int StudentId { get; set; }
        public int ScheduleId { get; set; }
    }
}
