namespace RegistrationManagementAPI.DTOs
{
    public class UpdateTeacherDTO
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Specialization { get; set; }
    }
}