using RegistrationManagementAPI.DTOs;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Services
{
    public interface IStudentService
    {
        Task<PaginatedList<StudentDTO>> GetStudentsPagedAsync(int pageNumber, int pageSize, string sortBy, bool isDescending);
        Task<StudentDTO> GetStudentByIdAsync(int id);
        Task<StudentDTO> AddStudentAsync(StudentDTO studentDTO);
        Task<StudentDTO> UpdateStudentAsync(int id, StudentDTO studentDTO);
        Task DeleteStudentAsync(int id);
    }
}
