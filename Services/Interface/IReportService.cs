using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IReportService
    {
        Task<object> GenerateRevenueReportAsync();
        Task<RegistrationReportDTO> GenerateRegistrationReportAsync();
        Task<List<TuitionFeeDTO>> GetTuitionFeesReportAsync();
        Task<List<SalaryDTO>> GetAllSalariesAsync();
        Task UpdateTuitionFee(int id ,TuitionFee tuitionFee);
        Task AddPaymentAndUpdateTuitionFeeAsync(int tuitionFeeId, Payment payment);
        Task UpdateSalaryAsync(int salaryId, SalaryDTO updatedSalary);
        Task PaySalaryAsync(int salaryId, SalaryDTO updatedSalary);
    }
}
