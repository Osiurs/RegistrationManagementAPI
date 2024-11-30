namespace RegistrationManagementAPI.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; } // Môn dạy chính
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        // Relationships
        public ICollection<Course> Courses { get; set; }
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}