using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDTO>> GetAttendancesByScheduleIdAsync(int scheduleId);
        Task AddAttendanceAsync(AttendanceDTO attendanceDTO);
        Task UpdateAttendanceAsync(int attendanceId, AttendanceDTO attendanceDTO);
    }
}
