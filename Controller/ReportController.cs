using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueReport()
        {
            var report = await _reportService.GenerateRevenueReportAsync();
            return Ok(report);
        }

        [HttpGet("registrations")]
        public async Task<IActionResult> GetRegistrationReport()
        {
            var report = await _reportService.GenerateRegistrationReportAsync();
            return Ok(report);
        }

        [HttpGet("tuition")]
        public async Task<IActionResult> GetTuitionReport()
        {
            var report = await _reportService.GenerateTuitionReportAsync();
            return Ok(report);
        }

        [HttpGet("salary")]
        public async Task<IActionResult> GetSalaryReport()
        {
            var report = await _reportService.GenerateSalaryReportAsync();
            return Ok(report);
        }
    }
}
