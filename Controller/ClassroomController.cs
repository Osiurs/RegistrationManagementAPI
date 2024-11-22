using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Services.Interface;

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

        [HttpGet]
        public async Task<IActionResult> GetAllClassrooms()
        {
            var classrooms = await _classroomService.GetAllClassroomsAsync();
            return Ok(classrooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassroomById(int id)
        {
            try
            {
                var classroom = await _classroomService.GetClassroomByIdAsync(id);
                return Ok(classroom);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("equipment/{equipment}")]
        public async Task<IActionResult> GetClassroomsWithEquipment(string equipment)
        {
            var classrooms = await _classroomService.GetClassroomsWithEquipmentAsync(equipment);
            return Ok(classrooms);
        }

        [HttpPost]
        public async Task<IActionResult> AddClassroom([FromBody] Classroom classroom)
        {
            try
            {
                var newClassroom = await _classroomService.AddClassroomAsync(classroom);
                return CreatedAtAction(nameof(GetClassroomById), new { id = newClassroom.ClassroomId }, newClassroom);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassroom(int id, [FromBody] Classroom classroom)
        {
            try
            {
                await _classroomService.UpdateClassroomAsync(id, classroom);
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
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            try
            {
                await _classroomService.DeleteClassroomAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
