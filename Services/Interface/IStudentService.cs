using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(int id, Student student);
        Task DeleteStudentAsync(int id);
    }
}
