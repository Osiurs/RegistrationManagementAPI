using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("schedule/{scheduleId}")]
        public async Task<IActionResult> GetAttendancesByScheduleId(int scheduleId)
        {
            var attendances = await _attendanceService.GetAttendancesByScheduleIdAsync(scheduleId);
            return Ok(attendances);
        }

        [HttpPost]
        public async Task<IActionResult> AddAttendance([FromBody] AttendanceDTO attendanceDTO)
        {
            await _attendanceService.AddAttendanceAsync(attendanceDTO);
            return CreatedAtAction(nameof(GetAttendancesByScheduleId), new { scheduleId = attendanceDTO.ScheduleId }, attendanceDTO);
        }

        [HttpPut("{attendanceId}")]
        public async Task<IActionResult> UpdateAttendance(int attendanceId, [FromBody] AttendanceDTO attendanceDTO)
        {
            // Set the AttendanceId in the DTO from the URL
            attendanceDTO.AttendanceId = attendanceId;

            await _attendanceService.UpdateAttendanceAsync(attendanceId, attendanceDTO);
            return NoContent();
        }

    }
}
