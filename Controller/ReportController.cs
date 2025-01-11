using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
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
            try
            {
                var tuitionFees = await _reportService.GetTuitionFeesReportAsync();
                return Ok(tuitionFees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("tuition/edit/{tuitionFeeId}")]
        public async Task<IActionResult> EditTuitionFee(int tuitionFeeId, [FromBody] TuitionFee updatedTuition)
        {
            if (updatedTuition == null)
            {
                return BadRequest(new { message = "Invalid data: updatedTuition is null" });
            }

            try
            {
                await _reportService.UpdateTuitionFee(tuitionFeeId, updatedTuition);
                return Ok(new { message = "TuitionFee updated successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("tuition/payment/{tuitionFeeId}")]
        public async Task<IActionResult> AddPaymentAndUpdateTuitionFee(
            int tuitionFeeId, [FromBody] Payment payment)
        {
            if (payment == null)
            {
                return BadRequest(new { message = "Payment data is required." });
            }

            try
            {
                // Gọi hàm xử lý từ service với thứ tự tham số chính xác
                await _reportService.AddPaymentAndUpdateTuitionFeeAsync(tuitionFeeId, payment);

                // Phản hồi thành công
                return Ok(new { message = "Payment added and tuition fee updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                // Xử lý lỗi không tìm thấy TuitionFee
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nội bộ
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("salary")]
        public async Task<ActionResult<List<SalaryDTO>>> GetSalaries()
        {
            var salaries = await _reportService.GetAllSalariesAsync();
            return Ok(salaries);
        }

        [HttpPut("salary/update/{id}")]
        public async Task<IActionResult> UpdateSalary(int id, [FromBody] SalaryDTO updatedSalary)
        {
            try
            {
                await _reportService.UpdateSalaryAsync(id, updatedSalary); // Chỉ gọi hàm service
                return Ok(new { message = "Salary updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("salary/pay/{id}")]
        public async Task<IActionResult> PaySalary(int id, [FromBody] SalaryDTO updatedSalary)
        {
            try
            {
                await _reportService.PaySalaryAsync(id, updatedSalary); // Chỉ gọi hàm service
                return Ok(new { message = "Pay Salary successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
