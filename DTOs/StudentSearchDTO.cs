namespace RegistrationManagementAPI.DTOs
{
    public class StudentSearchDTO
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ParentName { get; set; }
        public string ParentPhoneNumber { get; set; }
    }
}
