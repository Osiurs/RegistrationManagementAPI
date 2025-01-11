using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public AttendanceRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByScheduleIdAsync(int scheduleId)
        {
            return await _context.Attendances
                .Where(a => a.ScheduleId == scheduleId)
                .Include(a => a.Student)
                .ToListAsync();
        }

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int attendanceId)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Schedule)
                .FirstOrDefaultAsync(a => a.AttendanceId == attendanceId);
        }

        public async Task RemoveAttendancesByScheduleIdsAsync(List<int> scheduleIds)
        {
            var attendancesToRemove = await _context.Attendances
                .Where(a => scheduleIds.Contains(a.ScheduleId))
                .ToListAsync();

            if (attendancesToRemove.Any())
            {
                _context.Attendances.RemoveRange(attendancesToRemove);
                await _context.SaveChangesAsync();
            }
        }

    }
}
