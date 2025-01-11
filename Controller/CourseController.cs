using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Services.Interface;

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

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                return Ok(course);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseDTO courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (courseDto.TeacherId <= 0)
                return BadRequest(new { message = "TeacherId is required and must be greater than 0." });

            var createdCourse = await _courseService.AddCourseAsync(courseDto);
            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.CourseName }, createdCourse);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDTO courseDto)
        {
            try
            {
                await _courseService.UpdateCourseAsync(id, courseDto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetFeaturedCourses()
        {
            var courses = await _courseService.GetFeaturedCoursesAsync();

            if (courses == null || !courses.Any())
            {
                return NotFound("No featured courses available at the moment.");
            }

            return Ok(courses);
        }

        [HttpPut("increment-view/{id}")]
        public async Task<IActionResult> IncrementViewCount(int id)
        {
            var updatedCourse = await _courseService.IncrementViewCountAsync(id);

            if (updatedCourse == null)
            {
                return NotFound("Course not found or failed to update the view count.");
            }

            return Ok(updatedCourse); // Trả về đối tượng Course đã được cập nhật
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCoursesByUserId(int userId)
        {
            try
            {
                var courses = await _courseService.GetCoursesByUserIdAsync(userId);
                return Ok(courses);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("teacher/{userId}")]
        public async Task<IActionResult> GetCoursesByUserIdAsTeacher(int userId)
        {
            var courses = await _courseService.GetCoursesAsTeacherAsync(userId);

            if (courses == null || !courses.Any())
            {
                return NotFound("No featured courses available at the moment.");
            }

            return Ok(courses);
        }

        [HttpGet("student/{userId}")]
        public async Task<IActionResult> GetCoursesByStudentId(int userId)
        {
            try
            {
                var courses = await _courseService.GetCoursesByStudentAsync(userId);
                if (courses == null || !courses.Any())
                {
                    return NotFound(new { Message = "No courses found for this student." });
                }
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("course-members/{courseId}")]
        public async Task<IActionResult> GetCourseMembersAsync(int courseId)
        {
            try
            {
                var courseMembers = await _courseService.GetCourseMembersAsync(courseId);
                if (courseMembers == null || !courseMembers.Any())
                {
                    return NotFound("No members available at the moment.");
                }

                return Ok(courseMembers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("by-schedule/{id}")]
        public async Task<IActionResult> UpdateCourseBySchedule(int id, [FromBody] CourseDTO courseDto)
        {
            try
            {
                await _courseService.UpdateCourseByScheduleAsync(id, courseDto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("schedule-form")]
        public async Task<IActionResult> GetAllCoursesInScheduleFormAsync()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesInScheduleFormAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, "Internal server error");
            }
        }
        

        [HttpGet("schedule-form/{courseId}")]
        public async Task<IActionResult> GetCoursesInScheduleFormByIdAsync(int courseId)
        {
            try
            {
                var courses = await _courseService.GetCoursesInScheduleFormByIdAsync(courseId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
