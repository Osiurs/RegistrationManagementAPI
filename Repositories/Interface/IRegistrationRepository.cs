using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IRegistrationRepository
    {
        Task<IEnumerable<Registration>> GetAllRegistrationsAsync();
        Task<Registration> GetRegistrationByIdAsync(int id);
        Task<IEnumerable<Registration>> GetRegistrationsByStudentIdAsync(int studentId);
        Task<IEnumerable<Registration>> GetRegistrationsByCourseIdAsync(int courseId);
        Task<Registration> AddRegistrationAsync(Registration registration);
        Task UpdateRegistrationAsync(Registration registration);
        Task DeleteRegistrationAsync(int id);
    }
}
