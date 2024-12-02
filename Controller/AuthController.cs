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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDTO model)
        {
            try
            {
                await _authService.RegisterStudentAsync(model);
                return Created("", new { message = "Student registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register/teacher")]
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherDTO model)
        {
            try
            {
                await _authService.RegisterTeacherAsync(model);
                return Created("", new { message = "Teacher registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpPost("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO model)
        {
            try
            {
                var result = await _authService.ChangePasswordAsync(id, model);
                if (!result)
                    return BadRequest(new { message = "Failed to change password." });

                return Ok(new { message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
