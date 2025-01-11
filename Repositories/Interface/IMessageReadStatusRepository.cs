using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IMessageReadStatusRepository
    {
        Task AddMessageReadStatusAsync(MessageReadStatus messageReadStatus);
        Task<IEnumerable<MessageReadStatus>> GetUnreadMessagesByCourseIdAsync(int courseId);
        Task<IEnumerable<MessageReadStatus>> GetUnreadMessagesAsync(int userId);
        Task MarkMessagesAsReadAsync(int courseId, int userId);
        
    }
}
