using RegistrationManagementAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Services
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Registration>> GetAllRegistrationsAsync(); // Thêm phương thức này
        Task<Registration> GetRegistrationByIdAsync(int id);
        Task<IEnumerable<Registration>> GetRegistrationsByStudentIdAsync(int studentId);
        Task<Registration> AddRegistrationAsync(Registration registration);
        Task<Registration> UpdateRegistrationAsync(Registration registration);
        Task DeleteRegistrationAsync(int id);
    }
}
