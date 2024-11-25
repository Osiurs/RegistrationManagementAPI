using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDTO>> GetStudentScheduleAsync(int studentId);
    }
}
