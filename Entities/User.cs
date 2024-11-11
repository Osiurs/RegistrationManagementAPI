namespace RegistrationManagementAPI.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Admin", "Student", "Teacher"

        // Relationships
        public Student Student { get; set; } // Liên kết với bảng Student nếu là học viên
        public Teacher Teacher { get; set; } // Liên kết với bảng Teacher nếu là giáo viên
    }
}