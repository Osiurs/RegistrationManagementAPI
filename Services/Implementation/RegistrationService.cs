using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly NVHTNQ10DbContext _context;

        public RegistrationService(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
        {
            return await _context.Registrations
                .Include(r => r.Student)  // Include các thực thể liên quan nếu cần
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

        public async Task<Registration> AddRegistrationAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task<Registration> UpdateRegistrationAsync(Registration registration)
        {
            _context.Registrations.Update(registration);
            await _context.SaveChangesAsync();
            return registration;
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
