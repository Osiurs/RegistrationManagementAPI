using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Services;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // Lấy danh sách tất cả khóa học
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        // Lấy thông tin chi tiết của một khóa học theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }
            return Ok(course);
        }

        // Thêm khóa học mới
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCourse = await _courseService.AddCourseAsync(course);
            return CreatedAtAction(nameof(GetCourseById), new { id = newCourse.CourseId }, newCourse);
        }

        // Cập nhật thông tin khóa học
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCourse = await _courseService.UpdateCourseAsync(id, course);
            if (updatedCourse == null)
            {
                return NotFound(new { message = "Course not found" });
            }
            return Ok(updatedCourse);
        }

        // Xóa khóa học
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var existingCourse = await _courseService.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            await _courseService.DeleteCourseAsync(id);
            return NoContent();
        }
    }
}
