using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementations
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
            return await _reportRepository.GetRevenueReportAsync();
        }

        public async Task<RegistrationReportDTO> GenerateRegistrationReportAsync()
        {
            return await _reportRepository.GetRegistrationReportAsync();
        }

        public async Task<IEnumerable<TuitionReportDTO>> GenerateTuitionReportAsync()
        {
            return await _reportRepository.GetTuitionReportAsync();
        }

        public async Task<IEnumerable<SalaryReportDTO>> GenerateSalaryReportAsync()
        {
            return await _reportRepository.GetSalaryReportAsync();
        }
    }
}
