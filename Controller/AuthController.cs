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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var result = await _authService.RegisterAsync(model);
            return Ok(new { message = result });
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

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetDTO model)
        {
            var token = await _authService.GeneratePasswordResetTokenAsync(model.UserName);
            return Ok(new { token });
        }

        [HttpPost("confirm-password-reset")]
        public async Task<IActionResult> ConfirmPasswordReset(ConfirmPasswordResetDTO model)
        {
            var result = await _authService.ResetPasswordAsync(model);
            if (!result)
                return BadRequest(new { message = "Invalid token or user not found." });

            return Ok(new { message = "Password reset successful." });
        }
    }
}
