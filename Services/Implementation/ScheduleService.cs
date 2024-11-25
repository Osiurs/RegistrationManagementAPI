using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementations
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<IEnumerable<ScheduleDTO>> GetStudentScheduleAsync(int studentId)
        {
            var schedules = await _scheduleRepository.GetSchedulesByStudentIdAsync(studentId);

            return schedules.Select(s => new ScheduleDTO
            {
                CourseName = s.Course.CourseName,       // Tên khóa học
                StartDate = s.StartTime,               // Thời gian bắt đầu buổi học
                EndDate = s.EndTime,                   // Thời gian kết thúc buổi học
                RoomNumber = s.Classroom?.RoomNumber   // Lấy số phòng từ Classroom
            });
        }

    }
}
