using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Services;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy danh sách tất cả các khoản thanh toán.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            var paymentDTOs = _mapper.Map<IEnumerable<PaymentDTO>>(payments);
            return Ok(paymentDTOs);
        }

        /// <summary>
        /// Lấy danh sách các khoản thanh toán của một học viên.
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetPaymentsByStudentId(int studentId)
        {
            var payments = await _paymentService.GetPaymentsByStudentIdAsync(studentId);
            if (!payments.Any())
            {
                return NotFound(new { message = "No payments found for the student." });
            }

            var paymentDTOs = _mapper.Map<IEnumerable<PaymentDTO>>(payments);
            return Ok(paymentDTOs);
        }

        /// <summary>
        /// Lấy chi tiết một khoản thanh toán theo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }

            var paymentDTO = _mapper.Map<PaymentDTO>(payment);
            return Ok(paymentDTO);
        }

        /// <summary>
        /// Thêm một khoản thanh toán mới.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] PaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = _mapper.Map<Payment>(paymentDTO);
            var newPayment = await _paymentService.AddPaymentAsync(payment);
            var newPaymentDTO = _mapper.Map<PaymentDTO>(newPayment);

            return CreatedAtAction(nameof(GetPaymentById), new { id = newPaymentDTO.PaymentId }, newPaymentDTO);
        }

        /// <summary>
        /// Cập nhật thông tin một khoản thanh toán.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPayment = await _paymentService.GetPaymentByIdAsync(id);
            if (existingPayment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }

            var updatedPayment = _mapper.Map(paymentDTO, existingPayment);
            await _paymentService.UpdatePaymentAsync(updatedPayment);

            var updatedPaymentDTO = _mapper.Map<PaymentDTO>(updatedPayment);
            return Ok(updatedPaymentDTO);
        }

        /// <summary>
        /// Xóa một khoản thanh toán.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var existingPayment = await _paymentService.GetPaymentByIdAsync(id);
            if (existingPayment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }

            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }
    }
}
