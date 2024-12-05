using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;

        public CourseService(ICourseRepository courseRepository, ITeacherRepository teacherRepository)
        {
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
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

        public async Task<CourseDTO> AddCourseAsync(CourseDTO courseDto)
        {
            if (courseDto == null)
                throw new ArgumentNullException(nameof(courseDto), "Course data cannot be null");

            // Kiểm tra xem TeacherId có tồn tại hay không
            var teacher = await _teacherRepository.GetTeacherByTeacherIdAsync(courseDto.TeacherId);
            if (teacher == null)
                throw new Exception($"Teacher with ID {courseDto.TeacherId} does not exist");

            var course = new Course
            {
                CourseName = courseDto.CourseName,
                Price = courseDto.Price,
                StartDate = courseDto.StartDate,
                EndDate = courseDto.EndDate,
                TeacherId = courseDto.TeacherId // Bắt buộc phải có
            };

            var createdCourse = await _courseRepository.AddCourseAsync(course);

            return new CourseDTO
            {
                CourseName = createdCourse.CourseName,
                Price = createdCourse.Price,
                StartDate = createdCourse.StartDate,
                EndDate = createdCourse.EndDate,
                TeacherId = createdCourse.TeacherId
            };
        }


                public async Task<CourseDTO> UpdateCourseAsync(int id, CourseDTO courseDto)
        {
            var existingCourse = await _courseRepository.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            existingCourse.CourseName = courseDto.CourseName;
            existingCourse.Description = courseDto.Description;
            existingCourse.StartDate = courseDto.StartDate;
            existingCourse.EndDate = courseDto.EndDate;
            existingCourse.Price = courseDto.Price;
            existingCourse.TeacherId = courseDto.TeacherId;

            await _courseRepository.UpdateCourseAsync(existingCourse);
            return new CourseDTO
    {
        CourseName = existingCourse.CourseName,
        Price = existingCourse.Price,
        StartDate = existingCourse.StartDate,
        EndDate = existingCourse.EndDate,
        TeacherId = existingCourse.TeacherId
    };
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
