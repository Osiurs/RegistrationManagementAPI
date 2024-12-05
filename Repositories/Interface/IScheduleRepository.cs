using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<SetScheduleDTO>> GetStudentSchedulesAsync(int studentId);
        Task AddScheduleAsync(Schedule schedule);
        Task<Schedule> GetScheduleByIdAsync(int scheduleId);
        Task UpdateScheduleAsync(Schedule schedule);

    }
}
