using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Data;

namespace RegistrationManagementAPI.Services
{
    public class ClassroomService : IClassroomService
    {
        private readonly NVHTNQ10DbContext _context;

        public ClassroomService(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Classroom>> GetAllClassroomsAsync()
        {
            return await _context.Classrooms.ToListAsync();
        }

        public async Task<Classroom> AddClassroomAsync(Classroom classroom)
        {
            _context.Classrooms.Add(classroom);
            await _context.SaveChangesAsync();
            return classroom;
        }
    }
}
