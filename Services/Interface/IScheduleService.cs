using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IScheduleService
    {
        Task<IEnumerable<SetScheduleDTO>> GetAllScheduleAsync();
        Task<IEnumerable<SetScheduleDTO>> GetStudentScheduleAsync(int userId);
        Task SetScheduleAsync(SetScheduleRequestDTO request);
        Task UpdateScheduleAsync(UpdateScheduleRequestDTO request);
        Task<List<TeacherScheduleDTO>> GetTeachingScheduleAsync(int teacherId);
        Task<IEnumerable<Schedule>> GetScheduleByCourseIdAsync(int courseId);
        Task DeleteSchedulesByCourseIdAsync(int courseId);
    }
}
