using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IPaymentService
    {
        Task<List<PaymentDTO>> GetAllPaymentsAsync();
        Task<IEnumerable<PaymentDTO>> GetPaymentsByStudentIdAsync(int studentId);
        Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(int registrationId);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(int id, Payment payment);
        Task DeletePaymentAsync(int id);
    }
}
