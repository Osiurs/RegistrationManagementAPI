using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface ITeacherRepository
    {
        Task AddTeacherAsync(Teacher teacher);
        Task<Teacher> GetTeacherByEmailAsync(string email);
        Task<Teacher> GetTeacherByUserIdAsync(int userId);
    }
}
