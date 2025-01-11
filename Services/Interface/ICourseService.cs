using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
namespace RegistrationManagementAPI.Services.Interface
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCoursesAsync();
        Task<CourseDTO> GetCourseByIdAsync(int id);
        Task<IEnumerable<GetCourseByStudentDTO>> GetCoursesByUserIdAsync(int UserId);
        Task<IEnumerable<GetCourseByTeacherDTO>> GetCoursesAsTeacherAsync(int userId);
        Task<CourseDTO> AddCourseAsync(CourseDTO courseDto);
        Task<CourseDTO> UpdateCourseAsync(int id, CourseDTO courseDto);
        Task DeleteCourseAsync(int id);
        Task<IEnumerable<CourseDTO>> GetFeaturedCoursesAsync();
        Task<Course> IncrementViewCountAsync(int courseId);
        Task<IEnumerable<Registration>> GetCourseMembersAsync(int courseId);
        Task<List<GetCourseByStudentDTO>> GetCoursesByStudentAsync(int userId);
        Task<CourseDTO> UpdateCourseByScheduleAsync(int id, CourseDTO courseDto);
        Task<List<GetCourseByTeacherDTO>> GetAllCoursesInScheduleFormAsync();
        Task<GetCourseByTeacherWithClassRoomIdDTO> GetCoursesInScheduleFormByIdAsync(int courseId);
    }
}
