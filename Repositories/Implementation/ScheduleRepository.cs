using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
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

        public async Task<IEnumerable<SetScheduleDTO>> GetStudentSchedulesAsync(int studentId)
        {
            return await _context.Schedules
                .Where(s => s.StudentId == studentId) // Kiểm tra điều kiện StudentId
                .Select(s => new SetScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    CourseName = s.Course.CourseName,
                    TeacherName = $"{s.Teacher.FirstName} {s.Teacher.LastName}",
                    Classroom = $"{s.Classroom.RoomNumber} - {s.Classroom.Equipment}",
                    RoomNumber = s.Classroom.RoomNumber
                })
                .ToListAsync();
        }

        public async Task AddScheduleAsync(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<Schedule> GetScheduleByIdAsync(int scheduleId)
        {
            return await _context.Schedules
                                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
        }

    }
}
