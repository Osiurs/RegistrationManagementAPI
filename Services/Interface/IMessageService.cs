using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IMessageService
    {
        Task<MessageDTO> SendMessageAsync(int senderId, int receiverId, string content);
        Task<IEnumerable<MessageDTO>> GetMessagesByUserIdAsync(int userId);
    }

}