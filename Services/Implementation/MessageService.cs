using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;
using System.Linq;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageReadStatusRepository _messageReadStatusRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;

        public MessageService(IMessageRepository messageRepository, 
                                IMessageReadStatusRepository messageReadStatusRepository, 
                                ICourseRepository courseRepository,
                                IStudentRepository studentRepository)
        {
            _messageRepository = messageRepository;
            _messageReadStatusRepository = messageReadStatusRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
        }

        public async Task<MessageDTO> SendMessageToGroupAsync(int senderId, int courseId, string content)
        {
            // Lấy danh sách học viên trong khóa học từ repository
            var courseMembers = await _courseRepository.GetCourseMembersAsync(courseId);

            if (courseMembers == null || !courseMembers.Any())
                throw new ArgumentException("No members found in the course.");

            // Lấy thông tin khóa học để lấy TeacherId
            var course = await _courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
                throw new ArgumentException("Course not found.");

            var teacherId = course.TeacherId;  // TeacherId trong khóa học

            // Tạo một tin nhắn duy nhất cho nhóm
            var groupMessage = new Message
            {
                SenderId = senderId,
                CourseId = courseId,
                Content = content,
                SentDate = DateTime.UtcNow
            };

            // Lưu tin nhắn chung lên nhóm
            await _messageRepository.AddMessageAsync(groupMessage);

            // Sau khi lưu, lấy MessageId đã được sinh ra
            var savedMessage = await _messageRepository.GetMessageByIdAsync(groupMessage.MessageId);

            // Kiểm tra lại nếu tin nhắn không được lưu thành công
            if (savedMessage == null)
                throw new InvalidOperationException("Failed to save message.");

            // Gửi tin nhắn cho học sinh
            foreach (var member in courseMembers)
            {
                var student = await _studentRepository.GetStudentByIdAsync(member.StudentId);
                var messageReadStatus = new MessageReadStatus
                {
                    MessageId = savedMessage.MessageId,
                    UserId = student.UserId,  // Sử dụng UserId thay vì StudentId
                    IsRead = false  // Đánh dấu tin nhắn chưa đọc
                };

                // Kiểm tra xem MessageReadStatusId có tồn tại và cần thiết hay không
                if (messageReadStatus.MessageReadStatusId != 0)
                {
                    messageReadStatus.MessageReadStatusId = 0; // Hoặc bỏ hoặc set lại tùy cấu hình
                }

                // Lưu trạng thái đọc tin nhắn cho học sinh
                await _messageReadStatusRepository.AddMessageReadStatusAsync(messageReadStatus);
            }


            // Gửi tin nhắn cho giáo viên
            var messageReadStatusForTeacher = new MessageReadStatus
            {
                MessageId = savedMessage.MessageId,
                UserId = teacherId,  // Sử dụng TeacherId trực tiếp
                IsRead = false  // Đánh dấu tin nhắn chưa đọc
            };

            if (messageReadStatusForTeacher.MessageReadStatusId != 0)
                {
                    messageReadStatusForTeacher.MessageReadStatusId = 0; // Hoặc bỏ hoặc set lại tùy cấu hình
                }


            // Lưu trạng thái đọc tin nhắn cho giáo viên
            await _messageReadStatusRepository.AddMessageReadStatusAsync(messageReadStatusForTeacher);

            // Trả về DTO tin nhắn vừa gửi với MessageId
            return new MessageDTO
            {
                MessageId = savedMessage.MessageId,  // Trả về MessageId
                SenderId = senderId,
                CourseId = courseId,
                Content = content,
                SentDate = savedMessage.SentDate // Sử dụng SentDate của tin nhắn vừa tạo
            };
        }


        public async Task<IEnumerable<MessageDTO>> GetMessagesByCourseIdAsync(int courseId)
        {
            var messages = await _messageRepository.GetMessagesByCourseIdAsync(courseId);
            return messages.Select(m => new MessageDTO
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                CourseId = m.CourseId,
                Content = m.Content,
                SentDate = m.SentDate
            }).ToList();
        }

        public async Task MarkMessagesAsReadAsync(int courseId, int userId)
        {
            await _messageReadStatusRepository.MarkMessagesAsReadAsync(courseId, userId);
        }

        public async Task<IEnumerable<UnreadMessageDTO>> GetUnreadMessagesAsync(int userId)
        {
            var unreadMessages = await _messageReadStatusRepository.GetUnreadMessagesAsync(userId);

            // You will need to fetch the corresponding Message details (such as CourseId, Content, SentDate)
            return unreadMessages.Select(m => new UnreadMessageDTO
            {
                MessageId = m.MessageId,
                CourseId = m.Message?.CourseId ?? 0, // Ensure Message is fetched correctly
                Content = m.Message?.Content ?? string.Empty, // Access Content from Message
                SentDate = m.Message?.SentDate ?? DateTime.MinValue // Access SentDate from Message
            });
        }

        public async Task<IEnumerable<MessageDTO>> GetUnreadMessagesForCourseAsync(int courseId, int userId)
        {
            var unreadMessages = await _messageRepository.GetUnreadMessagesByCourseIdAsync(courseId, userId);

            // Convert to DTOs if necessary
            var messageDTOs = unreadMessages.Select(m => new MessageDTO
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                Content = m.Content,
                SentDate = m.SentDate
            }).ToList();

            return messageDTOs;
        }


    }
}
