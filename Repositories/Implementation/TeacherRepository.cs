using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public TeacherRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task AddTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
        }
        public async Task<Teacher> GetTeacherByEmailAsync(string email)
        {
            return await _context.Teachers.FirstOrDefaultAsync(t => t.Email == email);
        }
        public async Task<Teacher> GetTeacherByUserIdAsync(int userId)
        {
            return await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
        }

    }
}
