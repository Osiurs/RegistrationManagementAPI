using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IClassroomRepository
    {
        Task<IEnumerable<Classroom>> GetAllClassroomsAsync();
        Task<Classroom> GetClassroomByIdAsync(int id);
        Task<Classroom> AddClassroomAsync(Classroom classroom);
        Task UpdateClassroomAsync(Classroom classroom);
        Task DeleteClassroomAsync(int id);
        Task<IEnumerable<Classroom>> GetClassroomsWithEquipmentAsync(string equipment); // Tìm phòng có thiết bị cụ thể
    }
}
