using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class MessageRepository : IMessageRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public MessageRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesByUserIdAsync(int userId)
        {
            return await _context.Messages
                .Where(m => m.ReceiverId == userId)
                .ToListAsync();
        }


        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

    }
}
