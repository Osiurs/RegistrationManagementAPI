using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Services.Interface;

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
            var schedule = await _scheduleService.GetStudentScheduleAsync(studentId);
            return Ok(schedule);
        }
    }
}
