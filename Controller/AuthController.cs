using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent(RegisterStudentDTO model)
        {
            try
            {
                var result = await _authService.RegisterStudentAsync(model);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("register/teacher")]
        public async Task<IActionResult> RegisterTeacher(RegisterTeacherDTO model)
        {
            try
            {
                var result = await _authService.RegisterTeacherAsync(model);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var (token, role) = await _authService.LoginAsync(model);
            return Ok(new { token, role });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var result = await _authService.ChangePasswordAsync(userId, model);
            if (!result)
                return BadRequest(new { message = "Failed to change password." });

            return Ok(new { message = "Password changed successfully." });
        }
    }
}
