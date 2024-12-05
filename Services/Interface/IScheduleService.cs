using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IScheduleService
    {
        Task<IEnumerable<SetScheduleDTO>> GetStudentScheduleAsync(int studentId);
        Task SetScheduleAsync(SetScheduleRequestDTO request);
        Task UpdateScheduleAsync(int scheduleId, SetScheduleRequestDTO request);
    }
}
