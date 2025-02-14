using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("students")]
        public async Task<IActionResult> SearchStudents([FromQuery] string query)
        {
            var students = await _searchService.SearchStudentsAsync(query);
            return Ok(students);
        }

        [HttpGet("courses")]
        public async Task<IActionResult> SearchCourses([FromQuery] string query)
        {
            var courses = await _searchService.SearchCoursesAsync(query);
            return Ok(courses);
        }
        [HttpGet("teacher")]
        public async Task<IActionResult> SearchTeachers([FromQuery] string query)
        {
            try
            {
                var teachers = await _searchService.SearchTeachersAsync(query);
                return Ok(teachers);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
