using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IReportRepository
    {
        Task<RevenueReportDTO> GetRevenueReportAsync();
        Task<RegistrationReportDTO> GetRegistrationReportAsync();
        Task<IEnumerable<TuitionReportDTO>> GetTuitionReportAsync();
        Task<IEnumerable<SalaryReportDTO>> GetSalaryReportAsync();
    }
}
