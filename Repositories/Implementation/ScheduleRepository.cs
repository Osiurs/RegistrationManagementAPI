using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public ScheduleRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByStudentIdAsync(int studentId)
        {
            return await _context.Schedules
                .Include(s => s.Course)               // Bao gồm thông tin khóa học
                .Include(s => s.Classroom)            // Bao gồm thông tin phòng học
                .Where(s => s.Course.Registrations.Any(r => r.StudentId == studentId))
                .ToListAsync();
        }

    }
}
