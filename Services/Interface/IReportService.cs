using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IReportService
    {
        Task<RevenueReportDTO> GenerateRevenueReportAsync();
        Task<RegistrationReportDTO> GenerateRegistrationReportAsync();
    }
}
