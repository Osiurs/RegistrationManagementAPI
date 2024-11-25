using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<RevenueReportDTO> GenerateRevenueReportAsync()
        {
            var totalRevenue = await _reportRepository.GetTotalRevenueAsync();
            var totalPayments = await _reportRepository.GetTotalPaymentsAsync();

            return new RevenueReportDTO
            {
                TotalRevenue = totalRevenue,
                TotalPayments = totalPayments
            };
        }

        public async Task<RegistrationReportDTO> GenerateRegistrationReportAsync()
        {
            var totalRegistrations = await _reportRepository.GetTotalRegistrationsAsync();
            var activeRegistrations = await _reportRepository.GetActiveRegistrationsAsync();
            var completedRegistrations = await _reportRepository.GetCompletedRegistrationsAsync();

            return new RegistrationReportDTO
            {
                TotalRegistrations = totalRegistrations,
                ActiveRegistrations = activeRegistrations,
                CompletedRegistrations = completedRegistrations
            };
        }
    }
}
