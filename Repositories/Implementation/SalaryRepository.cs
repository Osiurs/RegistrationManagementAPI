using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public SalaryRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<List<SalaryDTO>> GetAllSalariesAsync()
        {
            return await _context.Salaries
                .Include(s => s.Teacher)
                .Select(s => new SalaryDTO
                {
                    SalaryId = s.SalaryId,
                    TeacherId = s.TeacherId,
                    TeacherName = s.Teacher.FirstName + " " + s.Teacher.LastName,
                    Specialization = s.Teacher.Specialization,
                    TotalSalary = s.Amount,
                    PaidAmount = s.PaidAmount,
                    Status = s.Status
                })
                .ToListAsync();
        }

        public async Task<Salary> GetSalaryByIdAsync(int salaryId)
        {
            // Lấy bản ghi Salary từ database dựa vào SalaryId
            return await _context.Salaries
                .FirstOrDefaultAsync(s => s.SalaryId == salaryId);
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            // Cập nhật thông tin Salary trong database
            _context.Salaries.Update(salary);
            await _context.SaveChangesAsync();
        }

        public async Task AddSalaryAsync(Salary salary)
        {
            // Thêm đối tượng Salary vào bảng Salaries
            _context.Salaries.Add(salary);
            
            // Lưu các thay đổi vào database
            await _context.SaveChangesAsync();
        }

        public async Task<Salary> GetSalaryByTeacherIdAsync(int teacherId)
        {
            // Fetch the Salary entity for the given teacherId
            return await _context.Salaries
                .Where(s => s.TeacherId == teacherId)  // Filter by TeacherId
                .FirstOrDefaultAsync();  // Return the first match, or null if not found
        }

    }
}
