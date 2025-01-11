using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ITuitionFeeRepository _tuitionFeeRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ISalaryRepository _salaryRepository;

        public ReportService(IReportRepository reportRepository, 
                            ITuitionFeeRepository tuitionFeeRepository,
                            IPaymentRepository paymentRepository,
                            IRegistrationRepository registrationRepository,
                            ISalaryRepository salaryRepository)
        {
            _reportRepository = reportRepository;
            _tuitionFeeRepository = tuitionFeeRepository;
            _paymentRepository = paymentRepository;
            _registrationRepository = registrationRepository;
            _salaryRepository = salaryRepository;
        }

        public async Task<object> GenerateRevenueReportAsync()
        {
            return await _reportRepository.GetRevenueReportAsync();
        }

        public async Task<RegistrationReportDTO> GenerateRegistrationReportAsync()
        {
            return await _reportRepository.GetRegistrationReportAsync();
        }

        public async Task<List<TuitionFeeDTO>> GetTuitionFeesReportAsync()
        {
            return await _tuitionFeeRepository.GetTuitionFeesAsync();
        }
        public async Task UpdateTuitionFee(int id, TuitionFee tuitionFee)
        {
            var existingTuition = await _tuitionFeeRepository.GetTuitionFeeById(id);
            if (existingTuition == null)
            {
                throw new InvalidOperationException("TuitionFee not found.");
            }
            // Cập nhật thông tin
            existingTuition.StudentId = tuitionFee.StudentId;
            existingTuition.Amount = tuitionFee.Amount;
            existingTuition.PaidAmount = tuitionFee.PaidAmount;
            existingTuition.Semester = tuitionFee.Semester;
            existingTuition.PaymentDeadline = tuitionFee.PaymentDeadline;
            existingTuition.Status = tuitionFee.Status;
            existingTuition.UpdatedAt = tuitionFee.UpdatedAt;
            existingTuition.CreatedAt = existingTuition.CreatedAt;

            await _tuitionFeeRepository.UpdateTuitionFee(existingTuition);
        }
        public async Task AddPaymentAndUpdateTuitionFeeAsync(int tuitionFeeId, Payment payment)
        {
            // Lấy thông tin TuitionFee dựa vào id
            var existingTuitionFee = await _tuitionFeeRepository.GetTuitionFeeById(tuitionFeeId);
            if (existingTuitionFee == null)
            {
                throw new InvalidOperationException("TuitionFee not found.");
            }

            // Tạo bản ghi Payment mới
            var newPayment = new Payment
            {
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate ?? DateTime.UtcNow,
                StudentId = existingTuitionFee.StudentId,
                PaymentMethod = payment.PaymentMethod
            };

            // Thêm bản ghi vào bảng Payment
            await _paymentRepository.AddPaymentAsync(newPayment);

            // Cập nhật PaidAmount của TuitionFee
            existingTuitionFee.PaidAmount += payment.Amount;

            // Cập nhật lại trạng thái (nếu cần)
            if (existingTuitionFee.PaidAmount >= existingTuitionFee.Amount)
            {
                existingTuitionFee.Status = "Paid"; // Hoặc trạng thái khác tùy thuộc logic
                var pendingRegistrations = await _registrationRepository.GetPendingRegistrationsByStudentIdAsync(existingTuitionFee.StudentId);
                foreach (var registration in pendingRegistrations)
                {
                    registration.Status = "Active";
                    await _registrationRepository.UpdateRegistrationAsync(registration);
                }
            }
            // Cập nhật thông tin
            existingTuitionFee.StudentId = existingTuitionFee.StudentId;
            existingTuitionFee.Amount = existingTuitionFee.Amount;
            existingTuitionFee.PaidAmount = existingTuitionFee.PaidAmount;
            existingTuitionFee.Semester = existingTuitionFee.Semester;
            existingTuitionFee.PaymentDeadline = existingTuitionFee.PaymentDeadline;
            existingTuitionFee.UpdatedAt = DateTime.Now;
            existingTuitionFee.CreatedAt = existingTuitionFee.CreatedAt;

            // Lưu thay đổi TuitionFee
            await _tuitionFeeRepository.UpdateTuitionFee(existingTuitionFee);
        }

        public async Task<List<SalaryDTO>> GetAllSalariesAsync()
        {
            return await _salaryRepository.GetAllSalariesAsync();
        }
        public async Task UpdateSalaryAsync(int salaryId, SalaryDTO updatedSalary)
        {
            var salary = await _salaryRepository.GetSalaryByIdAsync(salaryId);
            if (salary == null)
            {
                throw new InvalidOperationException("Salary not found.");
            }

            // Chuyển đổi từ DTO sang entity
            salary.TeacherId = salary.TeacherId;
            salary.Amount = updatedSalary.TotalSalary;
            salary.PaidAmount = updatedSalary.PaidAmount;
            salary.Status = updatedSalary.Status;
            salary.UpdatedAt = DateTime.UtcNow;

            await _salaryRepository.UpdateSalaryAsync(salary);
        }

        public async Task PaySalaryAsync(int salaryId, SalaryDTO updatedSalary)
        {
            var salary = await _salaryRepository.GetSalaryByIdAsync(salaryId);
            if (salary == null)
            {
                throw new InvalidOperationException("Salary not found.");
            }

            // Chuyển đổi từ DTO sang entity
            salary.TeacherId = salary.TeacherId;
            salary.Amount = salary.Amount;
            salary.PaidAmount = updatedSalary.PaidAmount;
            if(salary.PaidAmount >= salary.Amount)
                salary.Status = "Paid";
            else salary.Status ="Unpaid";
            salary.UpdatedAt = DateTime.UtcNow;

            await _salaryRepository.UpdateSalaryAsync(salary);
        }



    }
}
