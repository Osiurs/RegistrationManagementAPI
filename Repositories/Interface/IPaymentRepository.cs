using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment> AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int id);
    }
}
