using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RegistrationManagementAPI.Entities
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string ParentName { get; set; }

        [Required]
        [Phone]
        public string ParentPhoneNumber { get; set; }

        // Không bắt buộc khi thêm học viên, có thể null
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        // Không bắt buộc khi thêm học viên, có thể null
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
