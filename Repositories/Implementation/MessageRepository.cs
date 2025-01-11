using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class MessageRepository : IMessageRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public MessageRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByCourseIdAsync(int courseId)
        {
            return await _context.Messages
                .Where(m => m.CourseId == courseId)
                .Include(m => m.Sender) 
                .Include(m => m.Course)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == messageId);
        }

        public async Task UpdateMessageAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Message>> GetUnreadMessagesByCourseIdAsync(int courseId, int userId)
        {
            // Query unread messages by joining MessageReadStatus with Message
            var unreadMessages = await _context.Set<MessageReadStatus>()
                .Where(mrs => mrs.IsRead == false)  // Only unread messages
                .Where(mrs => mrs.UserId == userId)  // Filter by UserId to get messages for specific user
                .Join(
                    _context.Set<Message>(),  // Join with Message table
                    mrs => mrs.MessageId,  // MessageReadStatus.MessageId
                    m => m.MessageId,  // Message.MessageId
                    (mrs, m) => new { m, mrs }  // Create a projection of Message and MessageReadStatus
                )
                .Where(joined => joined.m.CourseId == courseId)  // Filter by CourseId in Message
                .Select(joined => joined.m)  // Select only the Message
                .ToListAsync();  // Return as a list

            return unreadMessages;
        }
        public async Task DeleteMessagesByCourseIdAsync(int courseId)
        {
            // Lấy danh sách các message theo CourseId
            var messages = await _context.Messages
                .Where(m => m.CourseId == courseId)
                .ToListAsync();

            // Xóa các message nếu tồn tại
            if (messages.Any())
            {
                _context.Messages.RemoveRange(messages);
                await _context.SaveChangesAsync();
            }
        }


    }
}
