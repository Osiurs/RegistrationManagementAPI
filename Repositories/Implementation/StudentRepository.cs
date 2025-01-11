using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class StudentRepository : IStudentRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public StudentRepository(NVHTNQ10DbContext context)
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
        public async Task<Student> GetStudentByUserIdAsync(int userId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        }


        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
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

        public List<Student> GetStudentsNotRegisteredInCourse(int courseId)
        {
            var registeredStudentIds = _context.Registrations
                .Where(r => r.CourseId == courseId)
                .Select(r => r.StudentId)
                .ToList();

            var studentsNotRegistered = _context.Students
                .Where(s => !registeredStudentIds.Contains(s.StudentId))
                .Include(s => s.User) // Load thêm dữ liệu User nếu cần
                .ToList();

            return studentsNotRegistered;
        }

    }
}
