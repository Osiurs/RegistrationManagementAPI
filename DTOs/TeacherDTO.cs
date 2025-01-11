namespace RegistrationManagementAPI.DTOs
{
    public class TeacherDTO
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; } // Môn dạy chính
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string TeacherName { get; set; }

    }
}