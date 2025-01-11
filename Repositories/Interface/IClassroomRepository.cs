using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IClassroomRepository
    {
        Task<List<ClassroomDTO>> GetAllClassroomsAsync();
        Task<ClassroomDTO> GetClassroomByIdAsync(int id);
        Task<Classroom> AddClassroomAsync(Classroom classroom);
        Task UpdateClassroomAsync(int id, Classroom classroom);
        Task DeleteClassroomAsync(int id);
        Task<IEnumerable<Classroom>> GetClassroomsWithEquipmentAsync(string equipment); // Tìm phòng có thiết bị cụ thể
        Task<Classroom> GetClassroomByIdToCheckAsync(int classroomId);
    }
}
