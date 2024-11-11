using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Services;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomService _classroomService;

        public ClassroomController(IClassroomService classroomService)
        {
            _classroomService = classroomService;
        }

        // Lấy danh sách tất cả phòng học
        [HttpGet]
        public async Task<IActionResult> GetAllClassrooms()
        {
            var classrooms = await _classroomService.GetAllClassroomsAsync();
            return Ok(classrooms);
        }

        // Thêm phòng học mới
        [HttpPost]
        public async Task<IActionResult> AddClassroom([FromBody] Classroom classroom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newClassroom = await _classroomService.AddClassroomAsync(classroom);
            return CreatedAtAction(nameof(GetAllClassrooms), new { id = newClassroom.ClassroomId }, newClassroom);
        }
    }
}
