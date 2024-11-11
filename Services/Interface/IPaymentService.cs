using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetPaymentsByStudentIdAsync(int studentId);
        Task<Payment> AddPaymentAsync(Payment payment);
    }
}
