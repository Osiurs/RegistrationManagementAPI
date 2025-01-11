using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesByCourseIdAsync(int courseId);
        Task<Message> GetMessageByIdAsync(int messageId);
        Task UpdateMessageAsync(Message message);
        Task<IEnumerable<Message>> GetUnreadMessagesByCourseIdAsync(int courseId, int userId);
        Task DeleteMessagesByCourseIdAsync(int courseId);
    }
}
