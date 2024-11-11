using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Services;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // Lấy danh sách đăng ký của một học viên theo ID học viên
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetRegistrationsByStudentId(int studentId)
        {
            var registrations = await _registrationService.GetRegistrationsByStudentIdAsync(studentId);
            return Ok(registrations);
        }

        // Đăng ký khóa học mới cho học viên
        [HttpPost]
        public async Task<IActionResult> AddRegistration([FromBody] Registration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newRegistration = await _registrationService.AddRegistrationAsync(registration);
            return CreatedAtAction(nameof(GetRegistrationsByStudentId), new { studentId = registration.StudentId }, newRegistration);
        }

        // Cập nhật trạng thái đăng ký khóa học (Pending, Confirmed, Canceled)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistrationStatus(int id, [FromBody] string status)
        {
            var registration = await _registrationService.GetRegistrationsByStudentIdAsync(id);
            if (registration == null)
            {
                return NotFound(new { message = "Registration not found" });
            }

            await _registrationService.UpdateRegistrationStatusAsync(id, status);
            return Ok(new { message = "Registration status updated successfully" });
        }
    }
}
