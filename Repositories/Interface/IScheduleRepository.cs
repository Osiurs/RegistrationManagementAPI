using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetSchedulesByStudentIdAsync(int studentId);
    }
}
