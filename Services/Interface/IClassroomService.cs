using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IClassroomService
    {
        Task<List<ClassroomDTO>> GetAllClassroomsAsync();
        Task<ClassroomDTO> GetClassroomByIdAsync(int id);
        Task<Classroom> AddClassroomAsync(Classroom classroom);
        Task UpdateClassroomAsync(int id, Classroom classroom);
        Task DeleteClassroomAsync(int id);
        Task<IEnumerable<Classroom>> GetClassroomsWithEquipmentAsync(string equipment); // Tìm phòng theo thiết bị
    }
}
