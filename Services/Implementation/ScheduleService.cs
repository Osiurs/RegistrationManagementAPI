using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassroomRepository _classroomRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, 
                       IClassroomRepository classroomRepository,
                       ICourseRepository courseRepository,
                       ITeacherRepository teacherRepository,
                       IStudentRepository studentRepository)

        {
            _scheduleRepository = scheduleRepository;
            _classroomRepository = classroomRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<SetScheduleDTO>> GetStudentScheduleAsync(int studentId)
        {
            return await _scheduleRepository.GetStudentSchedulesAsync(studentId);
        }

        public async Task SetScheduleAsync(SetScheduleRequestDTO request)
        {
            // Xác nhận ClassroomId tồn tại
            var classroomExists = await _classroomRepository.GetClassroomByIdAsync(request.ClassroomId);
            if (classroomExists == null)
            {
                throw new Exception($"Classroom with ID {request.ClassroomId} does not exist.");
            }

            var schedule = new Schedule
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                CourseId = request.CourseId,
                TeacherId = request.TeacherId,
                ClassroomId = request.ClassroomId
            };

            await _scheduleRepository.AddScheduleAsync(schedule);
        }

        public async Task UpdateScheduleAsync(int scheduleId, SetScheduleRequestDTO request)
        {
            // Kiểm tra sự tồn tại của Schedule
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(scheduleId);
            if (schedule == null)
            {
                throw new Exception($"Schedule with ID {scheduleId} not found.");
            }

            // Kiểm tra sự tồn tại của ClassroomId
            var classroomExists = await _classroomRepository.GetClassroomByIdAsync(request.ClassroomId);
            if (classroomExists == null)
            {
                throw new Exception($"Classroom with ID {request.ClassroomId} does not exist.");
            }

            // Kiểm tra sự tồn tại của CourseId nếu cần
            var courseExists = await _courseRepository.GetCourseByIdAsync(request.CourseId);
            if (courseExists == null)
            {
                throw new Exception($"Course with ID {request.CourseId} does not exist.");
            }

            // Kiểm tra sự tồn tại của TeacherId
            var teacherExists = await _teacherRepository.GetTeacherByTeacherIdAsync(request.TeacherId);
            if (teacherExists == null)
            {
                throw new Exception($"Teacher with ID {request.TeacherId} does not exist.");
            }
            var studentExists = await _studentRepository.GetStudentByIdAsync(request.StudentId);
            if (studentExists == null)
            {
                throw new Exception($"Student with ID {request.StudentId} does not exist.");
            }

            // Cập nhật thông tin Schedule
            schedule.StartTime = request.StartTime;
            schedule.EndTime = request.EndTime;
            schedule.CourseId = request.CourseId;
            schedule.TeacherId = request.TeacherId;
            schedule.ClassroomId = request.ClassroomId;
            schedule.StudentId = request.StudentId;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _scheduleRepository.UpdateScheduleAsync(schedule);
        }

    }
}
