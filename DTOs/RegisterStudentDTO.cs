using System.ComponentModel.DataAnnotations;

namespace RegistrationManagementAPI.DTOs
{
    public class RegisterStudentDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ParentName { get; set; }

        [Required]
        public string ParentPhoneNumber { get; set; }
    }
}
