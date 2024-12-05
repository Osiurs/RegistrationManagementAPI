using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<MessageDTO> SendMessageAsync(int senderId, int receiverId, string content)
        {
            // Kiểm tra người gửi và người nhận
            var sender = await _userRepository.GetUserByIdAsync(senderId);
            var receiver = await _userRepository.GetUserByIdAsync(receiverId);

            if (sender == null)
                throw new ArgumentException("Sender does not exist.");
            if (receiver == null)
                throw new ArgumentException("Receiver does not exist.");

            // Tạo đối tượng Message mới
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentDate = DateTime.UtcNow
            };

            // Lưu vào cơ sở dữ liệu qua repository
            await _messageRepository.AddMessageAsync(message);

            return new MessageDTO
            {
                MessageId = message.MessageId,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                SentDate = message.SentDate
            };
        }



        public async Task<IEnumerable<MessageDTO>> GetMessagesByUserIdAsync(int userId)
        {
            var messages = await _messageRepository.GetMessagesByUserIdAsync(userId);

            return messages.Select(m => new MessageDTO
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SentDate = m.SentDate
            }).ToList();
        }

    }
}
