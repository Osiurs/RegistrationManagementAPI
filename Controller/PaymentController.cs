using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Services;
using RegistrationManagementAPI.Entities;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Lấy danh sách thanh toán của học viên theo ID học viên
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetPaymentsByStudentId(int studentId)
        {
            var payments = await _paymentService.GetPaymentsByStudentIdAsync(studentId);
            return Ok(payments);
        }

        // Thêm thanh toán mới cho học viên
        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newPayment = await _paymentService.AddPaymentAsync(payment);
            return CreatedAtAction(nameof(GetPaymentsByStudentId), new { studentId = payment.StudentId }, newPayment);
        }
    }
}
