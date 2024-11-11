using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services
{
    public interface IClassroomService
    {
        Task<IEnumerable<Classroom>> GetAllClassroomsAsync();
        Task<Classroom> AddClassroomAsync(Classroom classroom);
    }
}
