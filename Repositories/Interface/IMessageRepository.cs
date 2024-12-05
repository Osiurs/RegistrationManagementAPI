using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesByUserIdAsync(int userId);
        Task AddMessageAsync(Message message);
    }
}
