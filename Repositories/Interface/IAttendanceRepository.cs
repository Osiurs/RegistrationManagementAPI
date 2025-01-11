using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAttendancesByScheduleIdAsync(int scheduleId);
        Task AddAttendanceAsync(Attendance attendance);
        Task UpdateAttendanceAsync(Attendance attendance);
        Task<Attendance> GetAttendanceByIdAsync(int attendanceId);
        Task RemoveAttendancesByScheduleIdsAsync(List<int> scheduleIds);
    }
}
