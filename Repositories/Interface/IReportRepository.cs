using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IReportRepository
    {
        Task<object> GetRevenueReportAsync();
        Task<RegistrationReportDTO> GetRegistrationReportAsync();
        Task<IEnumerable<TuitionReportDTO>> GetTuitionReportAsync();
        

    }
}
