using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Services.Interface;
using RegistrationManagementAPI.Entities;

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

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDTO model)
        {
            await _authService.RegisterAdminAsync(model);
            return Created("", new { message = "Admin registered successfully." });
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

        [Authorize]
        [HttpPatch("update-student/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDTO model)
        {
            try
            {
                // Extract the current user ID from the JWT token
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId;
                if (!int.TryParse(currentUserId, out userId))
                {
                    return Unauthorized(new { message = "Invalid User ID in the token." });
                }

                // Pass the currentUserId along with the student id to the service
                var result = await _authService.UpdateStudentInfoAsync(id, model, userId);
                if (!result)
                    return NotFound(new { message = "Student not found." });

                return Ok(new { message = "Student updated successfully." });
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating student info: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("update-teacher/{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] UpdateTeacherDTO model)
        {
            try
            {
                // Kiểm tra giá trị của model đầu vào
                if (model == null)
                {
                    return BadRequest(new { message = "Teacher data cannot be null." });
                }

                // Lấy UserId từ token
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(new { message = "User ID is missing in the token." });
                }

                int userId;
                if (!int.TryParse(currentUserId, out userId))
                {
                    return Unauthorized(new { message = "Invalid User ID in the token." });
                }

                // Gọi service để cập nhật thông tin giáo viên
                var result = await _authService.UpdateTeacherInfoAsync(id, model, userId);

                // Kiểm tra kết quả cập nhật
                if (!result)
                {
                    return NotFound(new { message = "Teacher not found." });
                }

                return Ok(new { message = "Teacher updated successfully." });
            }
            catch (Exception ex)
            {
                
                // Trả về lỗi 500 với thông báo chi tiết
                return StatusCode(500, new { message = "Internal Server Error: " + ex.Message });
            }
        }

        [HttpGet("user-details/{id}")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            try
            {
                var userDetails = await _authService.GetUserDetailsAsync(id);
                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("all-user-details")]
        public async Task<IActionResult> GetAllUserDetails()
        {
            try
            {
                var allUsers = await _authService.GetAllUserDetailsAsync();
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await _authService.DeleteUserByIdAsync(userId);
                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
