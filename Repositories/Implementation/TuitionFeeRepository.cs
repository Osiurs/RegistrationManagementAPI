using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class TuitionFeeRepository : ITuitionFeeRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public TuitionFeeRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<List<TuitionFeeDTO>> GetTuitionFeesAsync()
        {
            var tuitionFees = await _context.TuitionFees
                .Include(t => t.Student)  // Join với bảng Student để lấy tên sinh viên
                .Select(t => new TuitionFeeDTO
                {
                    TuitionId = t.TuitionId,
                    StudentId = t.StudentId,
                    StudentName = t.Student.FirstName + " " + t.Student.LastName,  // Giả sử Student có trường Name
                    TotalTuition = t.Amount,
                    PaidAmount = t.PaidAmount,
                    RemainingAmount = t.Amount - t.PaidAmount,
                    Status = t.Status,
                    PaymentDeadline = t.PaymentDeadline,
                    Semester = t.Semester
                })
                .ToListAsync();

            return tuitionFees;
        }
        public async Task<TuitionFee> GetTuitionFeeById(int id)
        {
            return await _context.TuitionFees
                .FirstOrDefaultAsync(t => t.TuitionId == id);
        }
        public async Task UpdateTuitionFee(TuitionFee tuitionFee)
        {
            _context.TuitionFees.Update(tuitionFee);
            await _context.SaveChangesAsync();
        }
        public async Task<TuitionFee> GetTuitionRecordByStudentIdAsync(int studentId)
        {
            // Retrieve the first tuition record for the student from the database
            var tuitionFee = await _context.TuitionFees
                .FirstOrDefaultAsync(t => t.StudentId == studentId);
                
            // Return the tuition fee record (null if not found)
            return tuitionFee;
        }
        public async Task<TuitionFee> AddTuitionFeeAsync(TuitionFee tuitionFee)
        {
            await _context.TuitionFees.AddAsync(tuitionFee);
            await _context.SaveChangesAsync();
            return tuitionFee; // Trả về đối tượng TuitionFee đã được lưu với ID tự động
        }

    }
}
