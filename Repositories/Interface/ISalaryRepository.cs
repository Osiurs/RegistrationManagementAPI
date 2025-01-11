using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface ISalaryRepository
    {
        Task<List<SalaryDTO>> GetAllSalariesAsync();
        Task<Salary> GetSalaryByIdAsync(int salaryId);
        Task UpdateSalaryAsync(Salary salary);
        Task<Salary> GetSalaryByTeacherIdAsync(int teacherId);
        Task AddSalaryAsync(Salary salary);
    }
}
