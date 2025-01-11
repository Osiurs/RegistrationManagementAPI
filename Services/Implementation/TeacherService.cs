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
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<IEnumerable<TeacherDTO>> GetAllTeachersAsync()
        {
            // Lấy tất cả giáo viên từ repository
            var teachers = await _teacherRepository.GetAllTeachersAsync();

            // Sử dụng LINQ để chuyển đổi từng giáo viên thành TeacherDTO
            var teacherDtos = teachers.Select(teacher => new TeacherDTO
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Specialization = teacher.Specialization,
                PhoneNumber = teacher.PhoneNumber,
                Email = teacher.Email,
                UserId = teacher.UserId,
                TeacherName = teacher.FirstName + " " + teacher.LastName // Gộp FirstName và LastName thành TeacherName
            });

            return teacherDtos;
        }

    }
}