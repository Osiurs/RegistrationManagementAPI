using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.DTOs;
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

        public async Task<object> GetRevenueReportAsync()
        {
            // Tính tổng toàn bộ số tiền (Amount) trong bảng Payment
            var totalRevenue = await _context.Payments.SumAsync(payment => payment.Amount);

            // Trả về kết quả chỉ chứa TotalRevenue
            return new
            {
                TotalRevenue = totalRevenue
            };
        }



        public async Task<RegistrationReportDTO> GetRegistrationReportAsync()
        {
            var totalRegistrations = await _context.Registrations.CountAsync();
            var activeRegistrations = await _context.Registrations.CountAsync(r => r.Status == "Active");
            var completedRegistrations = await _context.Registrations.CountAsync(r => r.Status == "Completed");

            return new RegistrationReportDTO
            {
                TotalRegistrations = totalRegistrations,
                ActiveRegistrations = activeRegistrations,
                CompletedRegistrations = completedRegistrations
            };
        }

        public async Task<IEnumerable<TuitionReportDTO>> GetTuitionReportAsync()
        {
            return await _context.Students
                .Select(s => new TuitionReportDTO
                {
                    StudentId = s.StudentId,
                    StudentName = $"{s.FirstName} {s.LastName}",
                    TotalTuition = s.Registrations.Sum(r => r.Course.Price),
                    PaidAmount = s.Payments.Sum(p => p.Amount),
                    RemainingAmount = s.Registrations.Sum(r => r.Course.Price) - s.Payments.Sum(p => p.Amount)
                })
                .ToListAsync();
        }

    }
}
