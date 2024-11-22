using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public PaymentRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Student)
                .Include(p => p.Registration)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStudentIdAsync(int studentId)
        {
            return await _context.Payments
                .Where(p => p.StudentId == studentId)
                .Include(p => p.Student)
                .Include(p => p.Registration)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(int registrationId)
        {
            return await _context.Payments
                .Where(p => p.RegistrationId == registrationId)
                .Include(p => p.Student)
                .Include(p => p.Registration)
                .ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Student)
                .Include(p => p.Registration)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
