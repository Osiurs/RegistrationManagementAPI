using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public RegistrationRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
        {
            return await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Course)
                .ToListAsync();
        }

        public async Task<Registration> GetRegistrationByIdAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Course)
                .FirstOrDefaultAsync(r => r.RegistrationId == id);
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByStudentIdAsync(int studentId)
        {
            return await _context.Registrations
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Student)
                .Include(r => r.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCourseIdAsync(int courseId)
        {
            return await _context.Registrations
                .Where(r => r.CourseId == courseId)
                .Include(r => r.Student)
                .Include(r => r.Course)
                .ToListAsync();
        }

        public async Task<Registration> AddRegistrationAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task UpdateRegistrationAsync(Registration registration)
        {
            _context.Registrations.Update(registration);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRegistrationAsync(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
                await _context.SaveChangesAsync();
            }
        }
    }
}
