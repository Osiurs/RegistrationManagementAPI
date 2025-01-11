using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface ICourseRepository
    {
        Task<IEnumerable<CourseDTO>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetCoursesByUserIdAsync(int studentId);
        Task<List<GetCourseByTeacherDTO>> GetCoursesByTeacherIdAsync(int teacherId);
        Task<Course> AddCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
        Task<IEnumerable<Course>> GetFeaturedCoursesAsync();
        Task<IEnumerable<Registration>> GetCourseMembersAsync(int courseId);
        Task<List<GetCourseByStudentDTO>> GetCoursesByStudentIdAsync(int studentId);
        Task<List<GetCourseByTeacherDTO>> GetAllCoursesInScheduleFormAsync();
        Task<GetCourseByTeacherWithClassRoomIdDTO> GetCoursesInScheduleFormWithClassIdByIdAsync(int courseId);
    }
}
