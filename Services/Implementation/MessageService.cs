using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessageAsync(MessageDTO messageDTO)
        {
            if (string.IsNullOrWhiteSpace(messageDTO.Title))
            {
                throw new ArgumentException("Message title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(messageDTO.Content))
            {
                throw new ArgumentException("Message content cannot be empty.");
            }

            var message = new Message
            {
                StudentId = messageDTO.StudentId,
                Title = messageDTO.Title,
                Content = messageDTO.Content,
                SentDate = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);
        }

        public async Task<IEnumerable<MessageDTO>> GetStudentMessagesAsync(int studentId)
        {
            var messages = await _messageRepository.GetMessagesByStudentIdAsync(studentId);

            return messages.Select(m => new MessageDTO
            {
                MessageId = m.MessageId,
                StudentId = m.StudentId,
                Title = m.Title,
                Content = m.Content,
                SentDate = m.SentDate
            });
        }
    }
}
