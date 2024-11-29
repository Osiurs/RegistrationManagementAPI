using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllCoursesAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }
            return course;
        }

        public async Task<IEnumerable<Course>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            return await _courseRepository.GetCoursesByTeacherIdAsync(teacherId);
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            if (course.StartDate >= course.EndDate)
            {
                throw new ArgumentException("Start date must be earlier than end date.");
            }

            if (course.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            return await _courseRepository.AddCourseAsync(course);
        }

        public async Task UpdateCourseAsync(int id, Course course)
        {
            var existingCourse = await _courseRepository.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            existingCourse.CourseName = course.CourseName;
            existingCourse.Description = course.Description;
            existingCourse.StartDate = course.StartDate;
            existingCourse.EndDate = course.EndDate;
            existingCourse.Price = course.Price;
            existingCourse.TeacherId = course.TeacherId;

            await _courseRepository.UpdateCourseAsync(existingCourse);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            await _courseRepository.DeleteCourseAsync(id);
        }
    }
}
