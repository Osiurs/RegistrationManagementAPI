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

        public async Task<IEnumerable<SetScheduleDTO>> GetAllScheduleAsync()
        {
            return await _context.Schedules
                .Select(s => new SetScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    CourseName = s.Course.CourseName,
                    TeacherName = $"{s.Course.Teacher.FirstName} {s.Course.Teacher.LastName}", // Kết hợp họ và tên giáo viên
                    Classroom = $"{s.Classroom.RoomNumber} - {s.Classroom.Equipment}", // Thông tin lớp học
                    RoomNumber = s.Classroom.RoomNumber,
                    ScheduleDate = s.ScheduleDate
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<SetScheduleDTO>> GetStudentSchedulesAsync(int studentId)
        {
            return await _context.Schedules
                .Where(s => s.Course.Registrations.Any(r => r.StudentId == studentId)) // Kiểm tra điều kiện thông qua liên kết với bảng Registrations
                .Select(s => new SetScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    CourseName = s.Course.CourseName,
                    TeacherName = $"{s.Course.Teacher.FirstName} {s.Course.Teacher.LastName}", // Lấy thông tin giáo viên từ bảng Course
                    Classroom = $"{s.Classroom.RoomNumber} - {s.Classroom.Equipment}", // Lấy thông tin từ bảng Classroom
                    RoomNumber = s.Classroom.RoomNumber,
                    ScheduleDate = s.ScheduleDate
                })
                .ToListAsync();
        }

        public async Task AddScheduleAsync(Schedule schedule)
        {
            // Đảm bảo không truyền TeacherId vào câu lệnh SQL
            _context.Schedules.Add(schedule);
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

        public async Task UpdateSchedulesAsync(IEnumerable<Schedule> schedulesToUpdate)
        {
            if (schedulesToUpdate == null || !schedulesToUpdate.Any())
                return;

            foreach (var schedule in schedulesToUpdate)
            {
                var existingSchedule = await _context.Schedules
                    .FirstOrDefaultAsync(s => s.ScheduleId == schedule.ScheduleId);

                if (existingSchedule != null)
                {
                    // Cập nhật thông tin từ schedule mới
                    existingSchedule.StartTime = schedule.StartTime;
                    existingSchedule.EndTime = schedule.EndTime;
                    existingSchedule.ClassroomId = schedule.ClassroomId;
                    existingSchedule.ScheduleDate = schedule.ScheduleDate;
                    existingSchedule.CourseId = schedule.CourseId;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveSchedulesAsync(IEnumerable<int> scheduleIds)
        {
            if (scheduleIds == null || !scheduleIds.Any())
                return;

            var schedulesToRemove = await _context.Schedules
                .Where(s => scheduleIds.Contains(s.ScheduleId))
                .ToListAsync();

            if (schedulesToRemove.Any())
            {
                _context.Schedules.RemoveRange(schedulesToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TeacherScheduleDTO>> GetTeachingScheduleByTeacherIdAsync(int teacherId)
        {
            try
            {
                var schedules = await _context.Schedules
                    .Where(s => s.Course.TeacherId == teacherId)
                    .Select(s => new TeacherScheduleDTO
                    {
                        ScheduleId = s.ScheduleId,
                        CourseName = s.Course.CourseName,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        RoomNumber = s.Classroom.RoomNumber,
                        ScheduleDate = s.ScheduleDate
                    })
                    .ToListAsync();

                return schedules ?? new List<TeacherScheduleDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTeachingScheduleByTeacherIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByCourseIdAsync(int courseId)
        {
            return await _context.Schedules
                .Where(s => s.CourseId == courseId)
                .ToListAsync();
        }

        public async Task DeleteSchedulesByCourseIdAsync(int courseId)
        {
            var schedulesToDelete = await _context.Schedules
                                                .Where(s => s.CourseId == courseId)
                                                .ToListAsync();

            if (schedulesToDelete.Any())
            {
                _context.Schedules.RemoveRange(schedulesToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public List<DayOfWeek> ParseNewDays(string dayOfWeekString)
        {
            return dayOfWeekString.Split(',')
                                .Select(day => day.Trim())
                                .Where(day => Enum.TryParse(day, true, out DayOfWeek _))
                                .Select(day => Enum.Parse<DayOfWeek>(day, true))
                                .ToList();
        }

        public List<DayOfWeek> GetExistingDays(IEnumerable<Schedule> schedules)
        {
            return schedules.Select(s => s.ScheduleDate.DayOfWeek)
                            .Distinct()
                            .ToList();
        }

            public (List<DayOfWeek> DaysToAdd, List<DayOfWeek> DaysToRemove) CompareDays(
                List<DayOfWeek> newDays, List<DayOfWeek> existingDays)
            {
                var daysToAdd = newDays.Except(existingDays).ToList();
                var daysToRemove = existingDays.Except(newDays).ToList();
                return (daysToAdd, daysToRemove);
            }

        public List<Schedule> FilterSchedulesByDays(IEnumerable<Schedule> schedules, List<DayOfWeek> days)
        {
            return schedules.Where(s => days.Contains(s.ScheduleDate.DayOfWeek))
                            .ToList();
        }

        public IEnumerable<int> ExtractScheduleIds(IEnumerable<Schedule> schedules)
        {
            return schedules.Select(schedule => schedule.ScheduleId).ToList();
        }

        // Lấy danh sách các lịch nằm ngoài phạm vi ngày
    public IEnumerable<Schedule> GetSchedulesOutsideDateRange(IEnumerable<Schedule> schedules, DateTime startDate, DateTime endDate)
    {
        return schedules.Where(s => s.ScheduleDate < startDate || s.ScheduleDate > endDate);
    }

    // Lấy danh sách lịch cần cập nhật trong phạm vi ngày
    public IEnumerable<Schedule> GetSchedulesToUpdate(IEnumerable<Schedule> schedules, IEnumerable<DayOfWeek> newDays, DateTime startDate, DateTime endDate)
    {
        // Chuyển đổi newDays từ IEnumerable<DayOfWeek> sang List<DayOfWeek>
        var daysList = newDays.ToList();
        return FilterSchedulesByDays(schedules, daysList)
            .Where(s => s.ScheduleDate >= startDate && s.ScheduleDate <= endDate);
    }


    // Kiểm tra lịch có tồn tại hay không trong danh sách hiện tại
    public bool IsScheduleExist(IEnumerable<Schedule> schedules, DateTime scheduleDate)
    {
        return schedules.Any(s => s.ScheduleDate == scheduleDate);
    }
    public bool IsScheduleDateValid(IEnumerable<Schedule> schedules, DayOfWeek day, DateTime loopDate)
    {
        return loopDate.DayOfWeek == day && !schedules.Any(s => s.ScheduleDate == loopDate);
    }

    public List<Schedule> GetExtraSchedules(IEnumerable<Schedule> schedules, DayOfWeek day, int extraCount)
    {
        return schedules
            .Where(s => s.ScheduleDate.DayOfWeek == day)
            .OrderBy(s => s.ScheduleDate)
            .Take(extraCount)
            .ToList();
    }

    public async Task RemoveSchedulesByIdsAsync(List<int> ids)
    {
        var schedulesToRemove = await _context.Set<Schedule>()
            .Where(s => ids.Contains(s.ScheduleId))
            .ToListAsync();

        _context.Set<Schedule>().RemoveRange(schedulesToRemove);
        await _context.SaveChangesAsync();
    }
    public async Task<int> GetScheduleCountByDayOfWeekAsync(int courseId, DayOfWeek dayOfWeek)
    {
        var schedules = await _context.Schedules
                                    .Where(s => s.CourseId == courseId && s.ScheduleDate.DayOfWeek == dayOfWeek)
                                    .ToListAsync();
        return schedules.Count;
    }
    public async Task<bool> HasScheduleOnDateAsync(int courseId, DateTime date)
    {
        var exists = await _context.Schedules
                                .AnyAsync(s => s.CourseId == courseId && s.ScheduleDate == date);
        return exists;
    }
    public async Task<IEnumerable<Schedule>> GetSchedulesByCourseIdToUpdateAsync(int courseId, int dayOfWeekValue)
    {
        return await _context.Schedules
            .Where(s => s.CourseId == courseId && s.DayOfWeekValue == dayOfWeekValue)
            .ToListAsync();
    }

    public async Task<Schedule> GetScheduleByCourseAndDateAsync(int courseId, DateTime scheduleDate, int dayOfWeekValue)
    {
        return await _context.Schedules
            .FirstOrDefaultAsync(s => s.CourseId == courseId 
                                    && s.ScheduleDate == scheduleDate 
                                    && s.DayOfWeekValue == dayOfWeekValue);
    }

    public async Task AddSchedulesAsync(IEnumerable<Schedule> schedules)
    {
        await _context.Schedules.AddRangeAsync(schedules);
        await _context.SaveChangesAsync();
    }

    }
}