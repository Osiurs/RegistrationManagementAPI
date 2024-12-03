using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<bool> UpdateStudentInfoAsync(int userId, UpdateStudentDTO model)
        {
            var student = await _studentRepository.GetStudentByUserIdAsync(userId);
            var user = await _userRepository.GetUserByIdAsync(userId);
            var existingStudent = await _studentRepository.GetStudentByEmailAsync(model.Email);
            if (existingStudent != null)
            {
                throw new Exception("A student with this email already exists.");
            }
            if (student == null) return false;
            if (!string.IsNullOrEmpty(model.UserName))user.UserName = model.UserName;
            if (!string.IsNullOrEmpty(model.FirstName)) student.FirstName = model.FirstName;
            if (!string.IsNullOrEmpty(model.LastName)) student.LastName = model.LastName;
            if (model.DateOfBirth.HasValue) student.DateOfBirth = model.DateOfBirth.Value;
            if (!string.IsNullOrEmpty(model.PhoneNumber)) student.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrEmpty(model.Email)) student.Email = model.Email;
            if (!string.IsNullOrEmpty(model.Address)) student.Address = model.Address;
            if (!string.IsNullOrEmpty(model.ParentName)) student.ParentName = model.ParentName;
            if (!string.IsNullOrEmpty(model.ParentPhoneNumber)) student.ParentPhoneNumber = model.ParentPhoneNumber;

            await _userRepository.UpdateUserAsync(user);
            await _studentRepository.UpdateStudentAsync(student);
            return true;
        }

        public async Task<bool> UpdateTeacherInfoAsync(int userId, UpdateTeacherDTO model)
        {
            var teacher = await _teacherRepository.GetTeacherByUserIdAsync(userId);
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (teacher == null) return false;
            if (!string.IsNullOrEmpty(model.UserName))user.UserName = model.UserName;
            if (!string.IsNullOrEmpty(model.FirstName)) teacher.FirstName = model.FirstName;
            if (!string.IsNullOrEmpty(model.LastName)) teacher.LastName = model.LastName;
            if (!string.IsNullOrEmpty(model.PhoneNumber)) teacher.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrEmpty(model.Email)) teacher.Email = model.Email;
            if (!string.IsNullOrEmpty(model.Specialization)) teacher.Specialization = model.Specialization;
            await _userRepository.UpdateUserAsync(user);
            await _teacherRepository.UpdateTeacherAsync(teacher);
            return true;
        }

        public async Task<object> GetUserDetailsAsync(int userId)
        {
            // Lấy thông tin từ bảng User
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            // Kiểm tra role và lấy thông tin chi tiết
            if (user.Role == "Student")
            {
                var student = await _studentRepository.GetStudentByUserIdAsync(userId);
                if (student == null) throw new Exception("Student details not found.");
                return new
                {
                    user.UserId,
                    user.UserName,
                    user.Role,
                    student.FirstName,
                    student.LastName,
                    student.Email,
                    student.PhoneNumber,
                    student.Address,
                    student.ParentName,
                    student.ParentPhoneNumber
                };
            }
            else if (user.Role == "Teacher")
            {
                var teacher = await _teacherRepository.GetTeacherByUserIdAsync(userId);
                if (teacher == null) throw new Exception("Teacher details not found.");
                return new
                {
                    user.UserId,
                    user.UserName,
                    user.Role,
                    teacher.FirstName,
                    teacher.LastName,
                    teacher.Email,
                    teacher.PhoneNumber,
                    teacher.Specialization
                };
            }

            throw new Exception("Invalid role.");
        }
        public async Task<List<object>> GetAllUserDetailsAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var students = await _studentRepository.GetAllStudentsAsync();
            var teachers = await _teacherRepository.GetAllTeachersAsync();

            var userDetails = new List<object>();

            foreach (var user in users)
            {
                if (user.Role == "Student")
                {
                    var student = students.FirstOrDefault(s => s.UserId == user.UserId);
                    userDetails.Add(new
                    {
                        user.UserId,
                        user.UserName,
                        user.Role,
                        FirstName = student?.FirstName,
                        LastName = student?.LastName,
                        Email = student?.Email,
                        PhoneNumber = student?.PhoneNumber,
                        Address = student?.Address,
                        ParentName = student?.ParentName,
                        ParentPhoneNumber = student?.ParentPhoneNumber
                    });
                }
                else if (user.Role == "Teacher")
                {
                    var teacher = teachers.FirstOrDefault(t => t.UserId == user.UserId);
                    userDetails.Add(new
                    {
                        user.UserId,
                        user.UserName,
                        user.Role,
                        FirstName = teacher?.FirstName,
                        LastName = teacher?.LastName,
                        Email = teacher?.Email,
                        PhoneNumber = teacher?.PhoneNumber,
                        Specialization = teacher?.Specialization
                    });
                }
            }

            return userDetails;
        }



    }
}
