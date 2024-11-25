using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesByStudentIdAsync(int studentId);
        Task AddMessageAsync(Message message);
    }
}
