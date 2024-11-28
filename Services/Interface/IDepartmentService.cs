using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();
        Task<DepartmentDTO> GetDepartmentByIdAsync(int id);
        Task<DepartmentDTO> AddDepartmentAsync(CreateUpdateDepartmentDTO departmentDto);
        Task UpdateDepartmentAsync(int id, CreateUpdateDepartmentDTO departmentDto);
        Task DeleteDepartmentAsync(int id);
    }
}
