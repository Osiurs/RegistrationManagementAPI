using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
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

        public async Task<List<ClassroomDTO>> GetAllClassroomsAsync()
        {
            return await _context.Classrooms
                .Include(c => c.Schedules) // Include bảng Schedules
                .ThenInclude(s => s.Course) // Include Course liên quan
                .ThenInclude(co => co.Teacher) // Include Teacher thông qua Course
                .Select(c => new ClassroomDTO
                {
                    ClassroomId = c.ClassroomId,
                    RoomNumber = c.RoomNumber,
                    Capacity = c.Capacity,
                    Equipment = c.Equipment,
                }).ToListAsync();
        }



        public async Task<ClassroomDTO> GetClassroomByIdAsync(int id)
        {
            return await _context.Classrooms
                .Include(c => c.Schedules)
                .ThenInclude(s => s.Course) // Include Course table
                .Select(c => new ClassroomDTO
                {
                    ClassroomId = c.ClassroomId,
                    RoomNumber = c.RoomNumber,
                    Capacity = c.Capacity,
                    Equipment = c.Equipment,
                    Schedules = c.Schedules.Select(s => new ScheduleDTO
                    {
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        CourseName = s.Course.CourseName // Assuming Course has a "Name" property
                    }).ToList()})
                .FirstOrDefaultAsync(c => c.ClassroomId == id);
        }

        public async Task<Classroom> GetClassroomByIdToCheckAsync(int classroomId)
        {
            return await _context.Classrooms
                                .AsNoTracking()
                                .Where(c => c.ClassroomId == classroomId)
                                .FirstOrDefaultAsync();
        }


        public async Task<Classroom> AddClassroomAsync(Classroom classroom)
        {
            _context.Classrooms.Add(classroom);
            await _context.SaveChangesAsync();
            return classroom;
        }
        public async Task UpdateClassroomAsync(int id, Classroom classroom)
        {
            var existingClassroom = await _context.Classrooms.FindAsync(id);
            if (existingClassroom == null)
            {
                throw new InvalidOperationException("Classroom not found");
            }

            // Update the fields that you want to change
            existingClassroom.RoomNumber = classroom.RoomNumber;
            existingClassroom.Capacity = classroom.Capacity;
            existingClassroom.Equipment = classroom.Equipment;
            // Save changes to the database
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
