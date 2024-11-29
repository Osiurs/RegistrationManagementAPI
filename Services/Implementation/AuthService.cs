using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDTO model)
        {
            var existingUser = await _userRepository.GetUserByUserNameAsync(model.UserName);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var newUser = new User
            {
                UserName = model.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = model.Role
            };

            await _userRepository.AddUserAsync(newUser);
            return "User registered successfully.";
        }

        public async Task<(string Token, string Role)> LoginAsync(LoginDTO model)
        {
            var user = await _userRepository.GetUserByUserNameAsync(model.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds);

            return (Token: new JwtSecurityTokenHandler().WriteToken(token), Role: user.Role);
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO model)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
                throw new UnauthorizedAccessException("Invalid current password.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userName)
        {
            var user = await _userRepository.GetUserByUserNameAsync(userName);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ResetPasswordAsync(ConfirmPasswordResetDTO model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                }, out var validatedToken);

                var userIdClaim = claimsPrincipal.FindFirst("UserId");
                if (userIdClaim == null)
                    return false;

                var userId = int.Parse(userIdClaim.Value);
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                    return false;

                user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                await _userRepository.UpdateUserAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
