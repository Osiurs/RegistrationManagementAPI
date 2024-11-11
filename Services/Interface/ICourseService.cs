using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<Course> AddCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(int id, Course course);
        Task DeleteCourseAsync(int id);
    }
}
