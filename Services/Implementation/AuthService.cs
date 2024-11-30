using Microsoft.IdentityModel.Tokens;
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
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            IConfiguration configuration)
            
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterStudentAsync(RegisterStudentDTO model)
        {
            var existingUser = await _userRepository.GetUserByUserNameAsync(model.UserName);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newUser = new User
            {
                UserName = model.UserName,
                Password = hashedPassword,
                Role = "Student"
            };

            await _userRepository.AddUserAsync(newUser);

            var student = new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Address = model.Address,
                ParentName = model.ParentName,
                ParentPhoneNumber = model.ParentPhoneNumber,
                UserId = newUser.UserId
            };

            await _studentRepository.AddStudentAsync(student);

            return "Student registered successfully.";
        }

        public async Task<string> RegisterTeacherAsync(RegisterTeacherDTO model)
        {
            var existingUser = await _userRepository.GetUserByUserNameAsync(model.UserName);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newUser = new User
            {
                UserName = model.UserName,
                Password = hashedPassword,
                Role = "Teacher"
            };

            await _userRepository.AddUserAsync(newUser);

            var teacher = new Teacher
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Specialization = model.Specialization,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserId = newUser.UserId
            };

            await _teacherRepository.AddTeacherAsync(teacher);

            return "Teacher registered successfully.";
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
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), user.Role);
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
    }
}
