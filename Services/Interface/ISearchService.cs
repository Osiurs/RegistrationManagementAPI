using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface ISearchService
    {
        Task<IEnumerable<StudentSearchDTO>> SearchStudentsAsync(string query);
        Task<IEnumerable<CourseSearchDTO>> SearchCoursesAsync(string query);
        Task<IEnumerable<Teacher>> SearchTeachersAsync(string query);
    }
}
