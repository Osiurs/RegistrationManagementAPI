namespace RegistrationManagementAPI.DTOs
{
    public class UpdateStudentDTO
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? ParentName { get; set; }
        public string? ParentPhoneNumber { get; set; }
    }

    
}
