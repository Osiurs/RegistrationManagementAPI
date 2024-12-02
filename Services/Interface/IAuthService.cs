using RegistrationManagementAPI.DTOs;

namespace RegistrationManagementAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<object> LoginAsync(LoginDTO model); 
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO model);
        Task RegisterStudentAsync(RegisterStudentDTO model);
        Task RegisterTeacherAsync(RegisterTeacherDTO model);
    }
}
