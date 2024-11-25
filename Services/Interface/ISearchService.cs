using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface ISearchService
    {
        Task<IEnumerable<StudentSearchDTO>> SearchStudentsAsync(string query);
        Task<IEnumerable<CourseSearchDTO>> SearchCoursesAsync(string query);
    }
}
