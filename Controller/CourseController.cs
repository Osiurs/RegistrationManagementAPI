using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Services;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CourseController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            var courseDTOs = _mapper.Map<IEnumerable<CourseDTO>>(courses);
            return Ok(courseDTOs);
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một khóa học theo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            var courseDTO = _mapper.Map<CourseDTO>(course);
            return Ok(courseDTO);
        }

        /// <summary>
        /// Thêm khóa học mới.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = _mapper.Map<Course>(courseDTO);
            var newCourse = await _courseService.AddCourseAsync(course);
            var newCourseDTO = _mapper.Map<CourseDTO>(newCourse);

            return CreatedAtAction(nameof(GetCourseById), new { id = newCourseDTO.CourseId }, newCourseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra sự tồn tại của khóa học
            var existingCourse = await _courseService.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                return NotFound(new { message = "Course not found." });
            }

            // Chuyển DTO thành Entity (Course)
            var updatedCourse = _mapper.Map(courseDTO, existingCourse);

            // Gọi phương thức UpdateCourseAsync từ Service
            await _courseService.UpdateCourseAsync(id, updatedCourse);

            // Trả về thông tin khóa học đã được cập nhật
            var updatedCourseDTO = _mapper.Map<CourseDTO>(updatedCourse);
            return Ok(updatedCourseDTO);
        }


        /// <summary>
        /// Xóa khóa học.
        /// </summary>
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
