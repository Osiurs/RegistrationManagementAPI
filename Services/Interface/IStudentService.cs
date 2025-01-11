using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> GetStudentByEmailAsync(string email);
        Task<Student> AddStudentAsync(Student student);
        Task UpdateStudentAsync(int id, Student student);
        Task DeleteStudentAsync(int id);
        List<Student> GetStudentsNotRegisteredInCourse(int courseId);
    }
}
