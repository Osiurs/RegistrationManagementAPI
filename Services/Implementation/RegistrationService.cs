using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly ITuitionFeeRepository _tuitionFeeRepository;
        public RegistrationService(IRegistrationRepository registrationRepository, 
                           IStudentRepository studentRepository,
                           ICourseRepository courseRepository,
                           ITuitionFeeRepository tuitionFeeRepository,
                           ISalaryRepository salaryRepository)
        {
            _registrationRepository = registrationRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _tuitionFeeRepository = tuitionFeeRepository;
            _salaryRepository = salaryRepository;
        }


        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
        {
            return await _registrationRepository.GetAllRegistrationsAsync();
        }

        public async Task<Registration> GetRegistrationByIdAsync(int id)
        {
            var registration = await _registrationRepository.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                throw new InvalidOperationException("Registration not found.");
            }
            return registration;
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByStudentIdAsync(int studentId)
        {
            return await _registrationRepository.GetRegistrationsByStudentIdAsync(studentId);
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCourseIdAsync(int courseId)
        {
            return await _registrationRepository.GetRegistrationsByCourseIdAsync(courseId);
        }

        public async Task<Registration> AddRegistrationAsync(Registration registration)
        {
            

            // Kiểm tra ngày đăng ký không được trong quá khứ
            if (registration.RegistrationDate < DateTime.UtcNow)
            {
                throw new ArgumentException("Registration date cannot be in the past.");
            }

            // Thêm đăng ký mới
            return await _registrationRepository.AddRegistrationAsync(registration);
        }


        public async Task UpdateRegistrationAsync(int id, Registration registration)
        {
            var existingRegistration = await _registrationRepository.GetRegistrationByIdAsync(id);
            if (existingRegistration == null)
            {
                throw new InvalidOperationException("Registration not found.");
            }

            existingRegistration.CourseId = registration.CourseId;
            existingRegistration.Status = registration.Status;
            existingRegistration.RegistrationDate = registration.RegistrationDate;

            await _registrationRepository.UpdateRegistrationAsync(existingRegistration);
        }

        public async Task DeleteRegistrationAsync(int id)
        {
            var registration = await _registrationRepository.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                throw new InvalidOperationException("Registration not found.");
            }

            await _registrationRepository.DeleteRegistrationAsync(id);
        }
        public async Task<RegistrationDTO> RegisterCourseAsync(RegistrationDTO registrationDto)
        {
            // Get the student by UserId
            var student = await _studentRepository.GetStudentByUserIdAsync(registrationDto.UserId);
            
            // Get the course details
            var course = await _courseRepository.GetCourseByIdAsync(registrationDto.CourseId);

            if (course == null)
            {
                throw new ArgumentException("Course not found.");
            }

            // Check if the student is already registered
            var existingRegistration = await _registrationRepository.GetPendingRegistrationsByStudentIdAndCourseIdAsync(student.StudentId, registrationDto.CourseId);

            if (existingRegistration != null)
            {
                // Create a detailed error message including the student ID and course ID
                string errorMessage = $"You are already registered. Student ID: {student.StudentId}, Course ID: {registrationDto.CourseId}";
                throw new ArgumentException(errorMessage);
            }

            // Create the Registration object
            var registration = new Registration
            {
                RegistrationDate = DateTime.Now,
                Status = registrationDto.Status,  // Default status as Pending
                StudentId = student.StudentId,
                CourseId = registrationDto.CourseId,
            };

            // Check if the student has already paid full tuition
            var tuitionRecord = await _tuitionFeeRepository.GetTuitionRecordByStudentIdAsync(student.StudentId);
            
            if (tuitionRecord != null)
            {
                // If the student has not paid in full
                if (tuitionRecord.PaidAmount < tuitionRecord.Amount)
                {
                    // Update Amount and Status for pending payment
                    tuitionRecord.Amount += course.Price;
                    tuitionRecord.Status = "Pending";
                }
                else
                {
                    // If the student has already paid, just update the Amount and set Status as Pending
                    tuitionRecord.Amount += course.Price;
                    tuitionRecord.Status = "Pending";
                }

                // Update tuition record with the new values
                await _tuitionFeeRepository.UpdateTuitionFee(tuitionRecord);
            }
            else
            {
                tuitionRecord = new TuitionFee
                {
                    StudentId = student.StudentId,
                    Amount = course.Price,
                    PaidAmount = 0, // Default paid amount is 0
                    Semester = "HK1", // Replace with actual semester logic
                    PaymentDeadline = DateTime.Now.AddMonths(1), // Set a default payment deadline
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                // Save the new tuition record to the database
                await _tuitionFeeRepository.AddTuitionFeeAsync(tuitionRecord);
            }

            // Retrieve all courses taught by the teacher
            var teacherCourses = await _courseRepository.GetCoursesByTeacherIdAsync(course.TeacherId);

            // Initialize the amount calculation for the teacher's salary
            decimal totalAmount = 0;

            foreach (var teacherCourse in teacherCourses)
            {
                // Get the active and completed students for each course
                var activeStudents = await _registrationRepository.GetActiveStudentsByCourseIdAsync(teacherCourse.CourseId);
                // Calculate the amount for each course
                var courseAmount = teacherCourse.Price * (activeStudents.Count());

                // Add the amount to the totalAmount
                totalAmount += courseAmount;
            }

            // Check if the Salary record exists for the teacher
            var teacherSalary = await _salaryRepository.GetSalaryByTeacherIdAsync(course.TeacherId);
            
            if (teacherSalary == null || (teacherSalary != null && teacherSalary.PaidAmount>=teacherSalary.Amount))
            {
               if (teacherSalary != null && teacherSalary.PaidAmount > teacherSalary.Amount)
                {
                    teacherSalary = new Salary
                    {
                        TeacherId = course.TeacherId,
                        Amount = totalAmount,
                        PaidAmount = totalAmount - teacherSalary.PaidAmount,
                        Status = (teacherSalary.PaidAmount >= totalAmount) ? "Paid" : "Unpaid", // Sửa lỗi ở đây
                        CreatedAt = DateTime.Now,
                        UpdatedAt = null
                    };
                }
                else
                {
                    teacherSalary = new Salary
                    {
                        TeacherId = course.TeacherId,
                        Amount = totalAmount,
                        PaidAmount = 0,
                        Status = "Unpaid", // Đặt trạng thái là Unpaid
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                }


                // Ensure AddSalaryAsync is implemented in ISalaryRepository
                await _salaryRepository.AddSalaryAsync(teacherSalary);
            }
            else
            {
                // If Salary record exists, update the amount
                teacherSalary.Amount = totalAmount;
                teacherSalary.Status = "Unpaid"; // Set status to Unpaid
                teacherSalary.UpdatedAt = DateTime.Now;

                await _salaryRepository.UpdateSalaryAsync(teacherSalary);
            }

            // Add the new course registration to the system
            var createdRegistration = await _registrationRepository.AddRegistrationAsync(registration);

            // Convert and return the registration details as a DTO
            return new RegistrationDTO
            {
                RegistrationId = createdRegistration.RegistrationId,
                RegistrationDate = createdRegistration.RegistrationDate,
                Status = createdRegistration.Status,
                StudentId = student.StudentId,
                CourseId = createdRegistration.CourseId,
            };
        }
        public async Task UpdateRegistrationsStatusToCompletedByCourseIdAsync(int courseId)
        {
            // Lấy danh sách đăng ký dựa vào CourseId
            var registrations = await _registrationRepository.GetRegistrationsByCourseIdAsync(courseId);

            if (registrations == null || !registrations.Any())
            {
                throw new InvalidOperationException("No registrations found for the specified course.");
            }

            // Lọc các bản ghi có trạng thái Active
            var activeRegistrations = registrations.Where(r => r.Status == "Active").ToList();

            if (!activeRegistrations.Any())
            {
                throw new InvalidOperationException("No active registrations found for the specified course.");
            }

            // Cập nhật trạng thái từng bản ghi
            foreach (var registration in activeRegistrations)
            {
                registration.Status = "Completed";
                await _registrationRepository.UpdateRegistrationAsync(registration);
            }
        }

    }
}
