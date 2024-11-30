using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<(string Token, string Role)> LoginAsync(LoginDTO model);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO model);
        Task<string> RegisterStudentAsync(RegisterStudentDTO model);
        Task<string> RegisterTeacherAsync(RegisterTeacherDTO model);
    }
}
