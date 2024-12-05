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

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentSchedule(int studentId)
        {
            var schedules = await _scheduleService.GetStudentScheduleAsync(studentId);
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

        [HttpPut("update/{scheduleId}")]
        public async Task<IActionResult> UpdateSchedule(int scheduleId, SetScheduleRequestDTO request)
        {
            try
            {
                await _scheduleService.UpdateScheduleAsync(scheduleId, request);
                return Ok(new { message = "Schedule updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
