using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<SetScheduleDTO>> GetAllScheduleAsync();
        Task<IEnumerable<SetScheduleDTO>> GetStudentSchedulesAsync(int studentId);
        Task AddScheduleAsync(Schedule schedule);
        Task<Schedule> GetScheduleByIdAsync(int scheduleId);
        Task UpdateScheduleAsync(Schedule schedule);
        Task UpdateSchedulesAsync(IEnumerable<Schedule> schedulesToUpdate);
        Task RemoveSchedulesAsync(IEnumerable<int> scheduleIds);
        Task<List<TeacherScheduleDTO>> GetTeachingScheduleByTeacherIdAsync(int teacherId);
        Task<IEnumerable<Schedule>> GetSchedulesByCourseIdAsync(int courseId);
        Task DeleteSchedulesByCourseIdAsync(int courseId);
        List<DayOfWeek> ParseNewDays(string dayOfWeekString);
        List<DayOfWeek> GetExistingDays(IEnumerable<Schedule> schedules);
        (List<DayOfWeek> DaysToAdd, List<DayOfWeek> DaysToRemove) CompareDays(List<DayOfWeek> newDays, List<DayOfWeek> existingDays);
        List<Schedule> FilterSchedulesByDays(IEnumerable<Schedule> schedules, List<DayOfWeek> days);
        IEnumerable<int> ExtractScheduleIds(IEnumerable<Schedule> schedules);
        bool IsScheduleDateValid(IEnumerable<Schedule> schedules, DayOfWeek day, DateTime loopDate);
        List<Schedule> GetExtraSchedules(IEnumerable<Schedule> schedules, DayOfWeek day, int extraCount);
        Task RemoveSchedulesByIdsAsync(List<int> ids);
        Task<bool> HasScheduleOnDateAsync(int courseId, DateTime date);
        Task<int> GetScheduleCountByDayOfWeekAsync(int courseId, DayOfWeek dayOfWeek);
        Task<IEnumerable<Schedule>> GetSchedulesByCourseIdToUpdateAsync(int courseId, int dayOfWeekValue);
        Task<Schedule> GetScheduleByCourseAndDateAsync(int courseId, DateTime scheduleDate, int dayOfWeekValue);
        Task AddSchedulesAsync(IEnumerable<Schedule> schedules);
    }
}
