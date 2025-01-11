using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class MessageReadStatusRepository : IMessageReadStatusRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public MessageReadStatusRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task AddMessageReadStatusAsync(MessageReadStatus messageReadStatus)
        {
            messageReadStatus.MessageReadStatusId = 0;
            _context.MessageReadStatus.Add(messageReadStatus);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<MessageReadStatus>> GetUnreadMessagesByCourseIdAsync(int courseId)
        {
            return await _context.MessageReadStatus
                .Where(mrs => mrs.Message.CourseId == courseId && !mrs.IsRead)
                .Include(mrs => mrs.Message)
                .ToListAsync();
        }

        public async Task<IEnumerable<MessageReadStatus>> GetUnreadMessagesAsync(int userId)
        {
            return await _context.MessageReadStatus
                .Where(mrs => mrs.UserId == userId && !mrs.IsRead)
                .Include(mrs => mrs.Message)
                .ToListAsync();
        }

        public async Task MarkMessagesAsReadAsync(int courseId, int userId)
        {
            // Bước 1: Truy vấn bảng Messages để lấy messageId của các tin nhắn thuộc khóa học tương ứng
            var messageIds = await _context.Messages
                .Where(m => m.CourseId == courseId)
                .Select(m => m.MessageId)
                .ToListAsync();

            // Bước 2: Truy vấn bảng MessageReadStatus để tìm các tin nhắn với messageId và userId tương ứng
            var messagesToMarkAsRead = await _context.MessageReadStatus
                .Where(mrs => messageIds.Contains(mrs.MessageId) && mrs.UserId == userId && !mrs.IsRead) // Chỉ lấy tin nhắn chưa đọc
                .ToListAsync();

            // Bước 3: Cập nhật trạng thái của các tin nhắn là đã đọc
            foreach (var message in messagesToMarkAsRead)
            {
                message.IsRead = true;
            }

            // Bước 4: Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }


    }
}
