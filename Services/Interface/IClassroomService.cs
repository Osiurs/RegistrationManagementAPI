using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IClassroomService
    {
        Task<IEnumerable<Classroom>> GetAllClassroomsAsync();
        Task<Classroom> GetClassroomByIdAsync(int id);
        Task<Classroom> AddClassroomAsync(Classroom classroom);
        Task UpdateClassroomAsync(int id, Classroom classroom);
        Task DeleteClassroomAsync(int id);
        Task<IEnumerable<Classroom>> GetClassroomsWithEquipmentAsync(string equipment); // Tìm phòng theo thiết bị
    }
}
