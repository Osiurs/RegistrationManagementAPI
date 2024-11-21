using RegistrationManagementAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync(); // Thêm định nghĩa
        Task<IEnumerable<Payment>> GetPaymentsByStudentIdAsync(int studentId);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int id);
    }
}
