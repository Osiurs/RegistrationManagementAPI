using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<PaymentDTO>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllPaymentsAsync();
        }


        public async Task<IEnumerable<PaymentDTO>> GetPaymentsByStudentIdAsync(int studentId)
        {
            var payments = await _paymentRepository.GetPaymentsByStudentIdAsync(studentId);
            if (payments == null || !payments.Any())
            {
                throw new KeyNotFoundException($"No payments found for student with ID {studentId}");
            }

            return payments;
        }


        public async Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(int registrationId)
        {
            return await _paymentRepository.GetPaymentsByRegistrationIdAsync(registrationId);
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                throw new InvalidOperationException("Payment not found.");
            }
            return payment;
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            if (payment.Amount <= 0)
            {
                throw new ArgumentException("Payment amount must be greater than zero.");
            }

            if (payment.PaymentDate > DateTime.UtcNow)
            {
                throw new ArgumentException("Payment date cannot be in the future.");
            }

            return await _paymentRepository.AddPaymentAsync(payment);
        }

        public async Task UpdatePaymentAsync(int id, Payment payment)
        {
            var existingPayment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (existingPayment == null)
            {
                throw new InvalidOperationException("Payment not found.");
            }

            existingPayment.Amount = payment.Amount;
            existingPayment.PaymentDate = payment.PaymentDate;
            existingPayment.PaymentMethod = payment.PaymentMethod;

            await _paymentRepository.UpdatePaymentAsync(existingPayment);
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                throw new InvalidOperationException("Payment not found.");
            }

            await _paymentRepository.DeletePaymentAsync(id);
        }
    }
}
