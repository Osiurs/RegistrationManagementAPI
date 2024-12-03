using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
namespace RegistrationManagementAPI.Services.Interface
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetCoursesByTeacherIdAsync(int teacherId);
        Task<CourseDTO> AddCourseAsync(CourseDTO courseDto);
        Task<CourseDTO> UpdateCourseAsync(int id, CourseDTO courseDto);
        Task DeleteCourseAsync(int id);
    }
}
