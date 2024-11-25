namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface IReportRepository
    {
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetTotalPaymentsAsync();
        Task<int> GetTotalRegistrationsAsync();
        Task<int> GetActiveRegistrationsAsync();
        Task<int> GetCompletedRegistrationsAsync();
    }
}
