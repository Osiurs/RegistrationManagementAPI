using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Data;  // Giả định DbContext của bạn nằm trong thư mục Data

namespace RegistrationManagementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly NVHTNQ10DbContext _context;

        public StudentService(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(int id, Student student)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return null;
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.Email = student.Email;
            existingStudent.ParentName = student.ParentName;

            await _context.SaveChangesAsync();
            return existingStudent;
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
    }
}
