using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task AddUserAsync(User user);
        Task<User> GetUserByUserNameAsync(string userName);
    }
}
