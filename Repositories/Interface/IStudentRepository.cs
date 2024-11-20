using RegistrationManagementAPI.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Repositories
{
    public interface IStudentRepository
    {
        IQueryable<Student> GetAllQueryable();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
    }
}
