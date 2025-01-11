namespace RegistrationManagementAPI.Entities
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }

        // Relationships
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
