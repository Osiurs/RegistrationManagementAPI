using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;

        public ClassroomService(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
        }

        public async Task<List<ClassroomDTO>> GetAllClassroomsAsync()
        {
            return await _classroomRepository.GetAllClassroomsAsync();
        }

        public async Task<ClassroomDTO> GetClassroomByIdAsync(int id)
        {
            var classroom = await _classroomRepository.GetClassroomByIdAsync(id);
            if (classroom == null)
            {
                throw new InvalidOperationException("Classroom not found.");
            }
            return classroom;
        }

        public async Task<Classroom> AddClassroomAsync(Classroom classroom)
        {
            if (classroom.Capacity <= 0)
            {
                throw new ArgumentException("Classroom capacity must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(classroom.RoomNumber))
            {
                throw new ArgumentException("Room number is required.");
            }

            return await _classroomRepository.AddClassroomAsync(classroom);
        }
    
        public async Task UpdateClassroomAsync(int id, Classroom classroom)
        {
            await _classroomRepository.UpdateClassroomAsync(id, classroom);
        }

        public async Task DeleteClassroomAsync(int id)
        {
            var classroom = await _classroomRepository.GetClassroomByIdAsync(id);
            if (classroom == null)
            {
                throw new InvalidOperationException("Classroom not found.");
            }

            await _classroomRepository.DeleteClassroomAsync(id);
        }

        public async Task<IEnumerable<Classroom>> GetClassroomsWithEquipmentAsync(string equipment)
        {
            if (string.IsNullOrWhiteSpace(equipment))
            {
                throw new ArgumentException("Equipment is required.");
            }

            return await _classroomRepository.GetClassroomsWithEquipmentAsync(equipment);
        }
    }
}
