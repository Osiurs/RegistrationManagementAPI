using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Data;

namespace RegistrationManagementAPI.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly NVHTNQ10DbContext _context;

        public RegistrationService(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByStudentIdAsync(int studentId)
        {
            return await _context.Registrations
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Course)
                .ToListAsync();
        }

        public async Task<Registration> AddRegistrationAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task UpdateRegistrationStatusAsync(int registrationId, string status)
        {
            var registration = await _context.Registrations.FindAsync(registrationId);
            if (registration != null)
            {
                registration.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
