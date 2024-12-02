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

        public async Task<object> LoginAsync(LoginDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                throw new Exception("Username and password are required.");
            }

            var user = await _userRepository.GetUserByUserNameAsync(model.UserName);

            if (user == null)
            {
                throw new Exception("Invalid username or password.");
            }

            // Giải mã mật khẩu
            var decodedPassword = DecodePasswordToken(user.Password);
            if (decodedPassword != model.Password)
            {
                throw new Exception("Invalid username or password.");
            }

            // Lấy thông tin FirstName và LastName từ Student hoặc Teacher
            string firstName = null;
            string lastName = null;

            if (user.Role == "Student")
            {
                var student = await _studentRepository.GetStudentByUserIdAsync(user.UserId);
                if (student != null)
                {
                    firstName = student.FirstName;
                    lastName = student.LastName;
                }
            }
            else if (user.Role == "Teacher")
            {
                var teacher = await _teacherRepository.GetTeacherByUserIdAsync(user.UserId);
                if (teacher != null)
                {
                    firstName = teacher.FirstName;
                    lastName = teacher.LastName;
                }
            }
            var token = GenerateToken(user);

            // Trả về dữ liệu cần thiết
            return new
            {
                UserID = user.UserId,
                FirstName = firstName,
                LastName = lastName,
                Role = user.Role,
                Token = token
            };
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Khóa phải đủ dài
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public async Task RegisterStudentAsync(RegisterStudentDTO model)
        {
            var existingUser = await _userRepository.GetUserByUserNameAsync(model.UserName);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var existingStudent = await _studentRepository.GetStudentByEmailAsync(model.Email);
            if (existingStudent != null)
            {
                throw new Exception("A student with this email already exists.");
            }

            var hashedPassword = GeneratePasswordToken(model.Password);

            var user = new User
            {
                UserName = model.UserName,
                Password = hashedPassword,
                Role = "Student"
            };

            user = await _userRepository.AddUserAsync(user);

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
                UserId = user.UserId
            };

            await _studentRepository.AddStudentAsync(student);
        }

        public async Task RegisterTeacherAsync(RegisterTeacherDTO model)
        {
            var existingUser = await _userRepository.GetUserByUserNameAsync(model.UserName);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var existingTeacher = await _teacherRepository.GetTeacherByEmailAsync(model.Email);
            if (existingTeacher != null)
            {
                throw new Exception("A teacher with this email already exists.");
            }

            var hashedPassword = GeneratePasswordToken(model.Password);

            var user = new User
            {
                UserName = model.UserName,
                Password = hashedPassword,
                Role = "Teacher"
            };

            user = await _userRepository.AddUserAsync(user);

            var teacher = new Teacher
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Specialization = model.Specialization,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserId = user.UserId
            };

            await _teacherRepository.AddTeacherAsync(teacher);
        }

         private string GeneratePasswordToken(string password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Hash, password) }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string DecodePasswordToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, parameters, out _);
            var passwordClaim = claimsPrincipal.FindFirst(ClaimTypes.Hash);
            return passwordClaim?.Value;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO model)
        {
            // Tìm kiếm người dùng theo ID
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            // Giải mã mật khẩu cũ từ token lưu trong cơ sở dữ liệu
            var decodedOldPassword = DecodePasswordToken(user.Password);

            // Kiểm tra xem mật khẩu cũ có khớp không
            if (decodedOldPassword != model.OldPassword)
                throw new Exception("Old password is incorrect.");

            // Mã hóa mật khẩu mới và cập nhật người dùng
            user.Password = GeneratePasswordToken(model.NewPassword);
            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        

    }
}
