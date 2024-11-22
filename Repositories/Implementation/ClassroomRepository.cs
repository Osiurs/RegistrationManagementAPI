using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public ClassroomRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Classroom>> GetAllClassroomsAsync()
        {
            return await _context.Classrooms
                .Include(c => c.Schedules) // Bao gồm liên kết với Schedules nếu cần
                .ToListAsync();
        }

        public async Task<Classroom> GetClassroomByIdAsync(int id)
        {
            return await _context.Classrooms
                .Include(c => c.Schedules)
                .FirstOrDefaultAsync(c => c.ClassroomId == id);
        }

        public async Task<Classroom> AddClassroomAsync(Classroom classroom)
        {
            _context.Classrooms.Add(classroom);
            await _context.SaveChangesAsync();
            return classroom;
        }

        public async Task UpdateClassroomAsync(Classroom classroom)
        {
            _context.Classrooms.Update(classroom);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassroomAsync(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom != null)
            {
                _context.Classrooms.Remove(classroom);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Classroom>> GetClassroomsWithEquipmentAsync(string equipment)
        {
            return await _context.Classrooms
                .Where(c => c.Equipment != null && c.Equipment.Contains(equipment))
                .ToListAsync();
        }
    }
}
