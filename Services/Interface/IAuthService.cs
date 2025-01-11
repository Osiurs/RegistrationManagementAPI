using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<object> LoginAsync(LoginDTO model); 
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO model);
        Task RegisterStudentAsync(RegisterStudentDTO model);
        Task RegisterTeacherAsync(RegisterTeacherDTO model);
        Task<bool> UpdateStudentInfoAsync(int userId, UpdateStudentDTO model, int currentUserId);
        Task<bool> UpdateTeacherInfoAsync(int userId, UpdateTeacherDTO model, int currentUserId);
        Task<object> GetUserDetailsAsync(int userId);
        Task<List<object>> GetAllUserDetailsAsync();
        Task DeleteUserByIdAsync(int userId);
        Task RegisterAdminAsync(RegisterAdminDTO model);
    }
}
