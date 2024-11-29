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

        public async Task<RevenueReportDTO> GetRevenueReportAsync()
        {
            var totalRevenue = await _context.Payments.SumAsync(p => p.Amount);
            var totalPayments = await _context.Payments.CountAsync();

            return new RevenueReportDTO
            {
                TotalRevenue = totalRevenue,
                TotalPayments = totalPayments
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

        public async Task<IEnumerable<SalaryReportDTO>> GetSalaryReportAsync()
        {
            return await _context.Teachers
                .Include(t => t.Courses) // Tải các khóa học liên quan của giáo viên
                .Select(t => new SalaryReportDTO
                {
                    TeacherId = t.TeacherId,
                    TeacherName = $"{t.FirstName} {t.LastName}",
                    TotalSalary = t.Courses.Sum(c => c.Price), // Tổng lương từ các khóa học
                    PaidAmount = 0, // Nếu không có thông tin lương đã trả, mặc định là 0
                    RemainingAmount = t.Courses.Sum(c => c.Price) // Số tiền còn lại = tổng lương
                })
                .ToListAsync();
        }

    }
}
