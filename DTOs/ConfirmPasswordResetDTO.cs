namespace RegistrationManagementAPI.DTOs
{
    public class ConfirmPasswordResetDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
