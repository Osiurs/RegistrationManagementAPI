using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<AttendanceDTO>> GetAttendancesByScheduleIdAsync(int scheduleId)
        {
            var attendances = await _attendanceRepository.GetAttendancesByScheduleIdAsync(scheduleId);

            return attendances.Select(a => new AttendanceDTO
            {
                AttendanceId = a.AttendanceId,
                AttendanceDate = a.AttendanceDate,
                Status  = a.Status ,
                StudentId = a.StudentId,
                ScheduleId = a.ScheduleId
            });
        }

        public async Task AddAttendanceAsync(AttendanceDTO attendanceDTO)
        {
            var attendance = new Attendance
            {
                AttendanceDate = DateTime.UtcNow,
                Status  = attendanceDTO.Status ,
                StudentId = attendanceDTO.StudentId,
                ScheduleId = attendanceDTO.ScheduleId
            };

            await _attendanceRepository.AddAttendanceAsync(attendance);
        }

        public async Task UpdateAttendanceAsync(int attendanceId, AttendanceDTO attendanceDTO)
        {
            var attendance = await _attendanceRepository.GetAttendanceByIdAsync(attendanceId);
            if (attendance == null) throw new ArgumentException("Attendance not found");

            attendance.Status  = attendanceDTO.Status ;
            attendance.AttendanceDate = attendance.AttendanceDate;

            await _attendanceRepository.UpdateAttendanceAsync(attendance);
        }
    }
}
