using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task AddTeacherAsync(Teacher teacher);
        Task<Teacher> GetTeacherByEmailAsync(string email);
        Task<Teacher> GetTeacherByUserIdAsync(int userId);
        Task<Teacher> GetTeacherByTeacherIdAsync(int teacherId);
        Task UpdateTeacherAsync(Teacher teacher);
    }
}
