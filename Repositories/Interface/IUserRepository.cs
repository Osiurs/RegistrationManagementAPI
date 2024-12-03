using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task<User> AddUserAsync(User user);
        Task<User> GetUserByUserNameAsync(string userName);
    }
}
