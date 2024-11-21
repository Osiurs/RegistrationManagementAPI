using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<PaginatedList<StudentDTO>> GetStudentsPagedAsync(int pageNumber, int pageSize, string sortBy, bool isDescending)
        {
            var students = _studentRepository.GetAllQueryable();

            students = isDescending
                ? students.OrderByDescending(s => EF.Property<object>(s, sortBy))
                : students.OrderBy(s => EF.Property<object>(s, sortBy));

            return await PaginatedList<StudentDTO>.CreateAsync(
                students.Select(s => new StudentDTO
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    DateOfBirth = s.DateOfBirth,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email,
                    Address = s.Address,
                    ParentName = s.ParentName,
                    ParentPhoneNumber = s.ParentPhoneNumber
                }),
                pageNumber, pageSize
            );
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return null;

            return new StudentDTO
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email,
                Address = student.Address,
                ParentName = student.ParentName,
                ParentPhoneNumber = student.ParentPhoneNumber
            };
        }

        public async Task<StudentDTO> AddStudentAsync(StudentDTO studentDTO)
{
    try
    {
        var student = new Student
        {
            StudentId = studentDTO.StudentId,
            FirstName = studentDTO.FirstName,
            LastName = studentDTO.LastName,
            DateOfBirth = studentDTO.DateOfBirth,
            PhoneNumber = studentDTO.PhoneNumber,
            Email = studentDTO.Email,
            Address = studentDTO.Address,
            ParentName = studentDTO.ParentName,
            ParentPhoneNumber = studentDTO.ParentPhoneNumber
        };

        var newStudent = await _studentRepository.AddStudentAsync(student);

        return new StudentDTO
        {
            FirstName = newStudent.FirstName,
            LastName = newStudent.LastName,
            DateOfBirth = newStudent.DateOfBirth,
            PhoneNumber = newStudent.PhoneNumber,
            Email = newStudent.Email,
            Address = newStudent.Address,
            ParentName = newStudent.ParentName,
            ParentPhoneNumber = newStudent.ParentPhoneNumber
        };
    }
    catch (Exception ex)
    {
        // Log the exception (or use any logging framework you are using)
        Console.WriteLine($"Error adding student: {ex.Message}");
        throw;  // Re-throw the exception to be handled at a higher level
    }
}


        public async Task<StudentDTO> UpdateStudentAsync(int id, StudentDTO studentDTO)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return null;

            student.FirstName = studentDTO.FirstName;
            student.LastName = studentDTO.LastName;
            student.DateOfBirth = studentDTO.DateOfBirth;
            student.PhoneNumber = studentDTO.PhoneNumber;
            student.Email = studentDTO.Email;
            student.Address = studentDTO.Address;
            student.ParentName = studentDTO.ParentName;
            student.ParentPhoneNumber = studentDTO.ParentPhoneNumber;

            var updatedStudent = await _studentRepository.UpdateStudentAsync(student);

            return new StudentDTO
            {
                FirstName = updatedStudent.FirstName,
                LastName = updatedStudent.LastName,
                DateOfBirth = updatedStudent.DateOfBirth,
                PhoneNumber = updatedStudent.PhoneNumber,
                Email = updatedStudent.Email,
                Address = updatedStudent.Address,
                ParentName = updatedStudent.ParentName,
                ParentPhoneNumber = updatedStudent.ParentPhoneNumber
            };
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteStudentAsync(id);
        }
    }
}
