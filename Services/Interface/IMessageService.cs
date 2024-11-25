using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IMessageService
    {
        Task SendMessageAsync(MessageDTO message);
        Task<IEnumerable<MessageDTO>> GetStudentMessagesAsync(int studentId);
    }

}