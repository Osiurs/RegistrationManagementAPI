using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class ReportRepository : IReportRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public ReportRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Payments.SumAsync(p => p.Amount);
        }

        public async Task<int> GetTotalPaymentsAsync()
        {
            return await _context.Payments.CountAsync();
        }

        public async Task<int> GetTotalRegistrationsAsync()
        {
            return await _context.Registrations.CountAsync();
        }

        public async Task<int> GetActiveRegistrationsAsync()
        {
            return await _context.Registrations.CountAsync(r => r.Status == "Active");
        }

        public async Task<int> GetCompletedRegistrationsAsync()
        {
            return await _context.Registrations.CountAsync(r => r.Status == "Completed");
        }
    }
}
