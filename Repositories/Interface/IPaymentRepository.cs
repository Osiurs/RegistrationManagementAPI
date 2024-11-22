using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<IEnumerable<Payment>> GetPaymentsByStudentIdAsync(int studentId);
        Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(int registrationId);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int id);
    }
}
