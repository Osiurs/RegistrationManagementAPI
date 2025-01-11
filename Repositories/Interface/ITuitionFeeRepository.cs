using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Repositories.Interface
{
    public interface ITuitionFeeRepository
    {
        Task<List<TuitionFeeDTO>> GetTuitionFeesAsync();
        Task<TuitionFee> GetTuitionFeeById(int id);
        Task UpdateTuitionFee(TuitionFee tuitionFee);
        Task<TuitionFee> GetTuitionRecordByStudentIdAsync(int studentId);
        Task<TuitionFee> AddTuitionFeeAsync(TuitionFee tuitionFee);
    }
}
