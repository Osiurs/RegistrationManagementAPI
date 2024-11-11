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

        // Relationships
        public ICollection<Course> Courses { get; set; }
    }
}