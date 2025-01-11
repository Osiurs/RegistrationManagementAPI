using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Services.Interface;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetAllScheduleAsync();
            return Ok(schedules);
        }

        [HttpGet("student/{userId}")]
        public async Task<IActionResult> GetStudentSchedule(int userId)
        {
            var schedules = await _scheduleService.GetStudentScheduleAsync(userId);
            if (!schedules.Any())
            {
                return NotFound(new { Message = "No schedules found for the given student ID." });
            }
            return Ok(schedules);
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetSchedule([FromBody] SetScheduleRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Invalid request data." });
            }

            await _scheduleService.SetScheduleAsync(request);
            return Ok(new { Message = "Schedule created successfully." });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSchedule(UpdateScheduleRequestDTO request)
        {
            try
            {
                // Call the service to perform the update
                await _scheduleService.UpdateScheduleAsync(request);

                // Return success response
                return Ok(new { message = "Schedule updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 for not found resources
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // Return 400 for invalid arguments
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return 500 for unexpected errors
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpGet("teacher/{userId}")]
        public async Task<IActionResult> GetTeachingSchedule(int userId)
        {
            try
            {
                var schedules = await _scheduleService.GetTeachingScheduleAsync(userId);
                if (schedules == null || !schedules.Any())
                {
                    return NotFound(new { message = "No teaching schedule found for the teacher." });
                }

                return Ok(schedules);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTeachingSchedule: {ex.Message}");
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("dates/{courseId}")]
        public async Task<IActionResult> GetScheduleDates(int courseId)
        {
            try
            {
                var scheduleDates = await _scheduleService.GetScheduleByCourseIdAsync(courseId);
                return Ok(scheduleDates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteSchedule(int courseId)
        {
            try
            {
                // Gọi service để xóa các lịch học theo courseId
                await _scheduleService.DeleteSchedulesByCourseIdAsync(courseId);
                return Ok(new { message = "Schedules deleted successfully" });
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có ngoại lệ
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
