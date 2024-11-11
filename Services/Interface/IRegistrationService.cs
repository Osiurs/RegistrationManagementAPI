using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Registration>> GetRegistrationsByStudentIdAsync(int studentId);
        Task<Registration> AddRegistrationAsync(Registration registration);
        Task UpdateRegistrationStatusAsync(int registrationId, string status);
    }
}
