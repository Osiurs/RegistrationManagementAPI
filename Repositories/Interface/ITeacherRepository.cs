using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface ITeacherRepository
    {
        Task AddTeacherAsync(Teacher teacher);
    }
}
