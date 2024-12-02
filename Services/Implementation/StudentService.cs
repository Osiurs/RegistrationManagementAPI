using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }
            return student;
        }

        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            var student = await _studentRepository.GetStudentByEmailAsync(email);
             if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }
            return student;
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName))
            {
                throw new ArgumentException("First name and last name are required.");
            }

            var existingStudents = await _studentRepository.GetAllStudentsAsync();
            if (existingStudents.Any(s => s.Email == student.Email))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            return await _studentRepository.AddStudentAsync(student);
        }

        public async Task UpdateStudentAsync(int id, Student student)
        {
            var existingStudent = await _studentRepository.GetStudentByIdAsync(id);
            if (existingStudent == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.DateOfBirth = student.DateOfBirth;

            await _studentRepository.UpdateStudentAsync(existingStudent);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            await _studentRepository.DeleteStudentAsync(id);
        }
    }
}
