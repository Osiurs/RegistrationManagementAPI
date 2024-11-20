using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RegistrationManagementAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public StudentRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public IQueryable<Student> GetAllQueryable()
        {
            return _context.Students.AsQueryable();
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

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
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
