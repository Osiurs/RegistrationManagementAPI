using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface ISearchRepository
    {
        Task<IEnumerable<Student>> SearchStudentsAsync(string query);
        Task<IEnumerable<Course>> SearchCoursesAsync(string query);
    }
}
