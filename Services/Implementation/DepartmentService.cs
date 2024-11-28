using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllDepartmentsAsync();
            return departments.Select(d => new DepartmentDTO
            {
                DepartmentId = d.DepartmentId,
                Name = d.Name,
                Description = d.Description
            });
        }

        public async Task<DepartmentDTO> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                throw new KeyNotFoundException("Department not found.");
            }

            return new DepartmentDTO
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                Description = department.Description
            };
        }

        public async Task<DepartmentDTO> AddDepartmentAsync(CreateUpdateDepartmentDTO departmentDto)
        {
            var department = new Department
            {
                Name = departmentDto.Name,
                Description = departmentDto.Description
            };

            var newDepartment = await _departmentRepository.AddDepartmentAsync(department);

            return new DepartmentDTO
            {
                DepartmentId = newDepartment.DepartmentId,
                Name = newDepartment.Name,
                Description = newDepartment.Description
            };
        }

        public async Task UpdateDepartmentAsync(int id, CreateUpdateDepartmentDTO departmentDto)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                throw new KeyNotFoundException("Department not found.");
            }

            department.Name = departmentDto.Name;
            department.Description = departmentDto.Description;

            await _departmentRepository.UpdateDepartmentAsync(department);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            await _departmentRepository.DeleteDepartmentAsync(id);
        }
    }
}
