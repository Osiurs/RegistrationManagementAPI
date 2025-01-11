using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IMessageService
    {
        Task<MessageDTO> SendMessageToGroupAsync(int senderId, int courseId, string content);
        Task<IEnumerable<MessageDTO>> GetMessagesByCourseIdAsync(int courseId);
        Task MarkMessagesAsReadAsync(int courseId, int userId);
        Task<IEnumerable<UnreadMessageDTO>> GetUnreadMessagesAsync(int userId);
        Task<IEnumerable<MessageDTO>> GetUnreadMessagesForCourseAsync(int courseId, int userId);
        
    }
}