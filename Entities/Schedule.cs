namespace RegistrationManagementAPI.Entities
{
    public class Schedule
{
    public int ScheduleId { get; set; }
    public int CourseId { get; set; }
    public DateTime ScheduleDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int ClassroomId { get; set; }
    public int DayOfWeekValue { get; set; }

    // Navigation Properties
    public Course Course { get; set; }
    public Classroom Classroom { get; set; }
}

}