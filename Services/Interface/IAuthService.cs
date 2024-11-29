using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<(string Token, string Role)> LoginAsync(LoginDTO model);
        Task<string> RegisterAsync(RegisterDTO model);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO model);
        Task<string> GeneratePasswordResetTokenAsync(string userName);
        Task<bool> ResetPasswordAsync(ConfirmPasswordResetDTO model);
    }
}
