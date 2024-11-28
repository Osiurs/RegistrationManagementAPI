namespace RegistrationManagementAPI.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        // Relationships
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Registration> Registrations { get; set; }
        public ICollection<Schedule> Schedules { get; set; }  // Thời khóa biểu
    }
}
