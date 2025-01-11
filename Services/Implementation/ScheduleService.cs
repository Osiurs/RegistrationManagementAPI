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
        private readonly IAttendanceRepository _attendanceRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, 
                       IClassroomRepository classroomRepository,
                       ICourseRepository courseRepository,
                       ITeacherRepository teacherRepository,
                       IStudentRepository studentRepository,
                       IAttendanceRepository attendanceRepository)

        {
            _scheduleRepository = scheduleRepository;
            _classroomRepository = classroomRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<SetScheduleDTO>> GetAllScheduleAsync()
        {
            return await _scheduleRepository.GetAllScheduleAsync();
        }

        public async Task<IEnumerable<SetScheduleDTO>> GetStudentScheduleAsync(int userId)
        {
            var student = await _studentRepository.GetStudentByUserIdAsync(userId);

            return await _scheduleRepository.GetStudentSchedulesAsync(student.StudentId);
        }

        public async Task SetScheduleAsync(SetScheduleRequestDTO request)
        {
            // Chuyển chuỗi "Monday", "Tuesday", etc. thành DayOfWeek enum
            if (!Enum.TryParse(request.DayOfWeek, true, out DayOfWeek dayOfWeek))
            {
                throw new Exception($"Invalid day of the week: {request.DayOfWeek}");
            }

            // Lấy giá trị số nguyên của DayOfWeek
            int dayOfWeekValue = (int)dayOfWeek;

            // Kiểm tra lớp học
            var classroomExists = await _classroomRepository.GetClassroomByIdToCheckAsync(request.ClassroomId);
            if (classroomExists == null)
            {
                throw new Exception($"Classroom with ID {request.ClassroomId} does not exist.");
            }

            // Kiểm tra khóa học
            var course = await _courseRepository.GetCourseByIdAsync(request.CourseId);
            if (course == null)
            {
                throw new Exception($"Course with ID {request.CourseId} does not exist.");
            }

            var currentDate = course.StartDate;

            // Lặp qua từng ngày và thêm từng lịch vào cơ sở dữ liệu
            var schedules = new List<Schedule>();
            while (currentDate <= course.EndDate)
            {
                if ((int)currentDate.DayOfWeek == dayOfWeekValue)
                {
                    // Kiểm tra xem lịch học đã tồn tại chưa
                    var existingSchedule = await _scheduleRepository.GetScheduleByCourseAndDateAsync(request.CourseId, currentDate, dayOfWeekValue);
                    if (existingSchedule != null)
                    {
                        // Nếu đã tồn tại, bỏ qua
                        currentDate = currentDate.AddDays(1);
                        continue;
                    }

                    // Tạo mới Schedule
                    var schedule = new Schedule
                    {
                        CourseId = request.CourseId,
                        ScheduleDate = currentDate,
                        StartTime = request.StartTime,
                        EndTime = request.EndTime,
                        ClassroomId = request.ClassroomId,
                        DayOfWeekValue = dayOfWeekValue
                    };

                    schedules.Add(schedule);
                }

                currentDate = currentDate.AddDays(1);
            }

            // Thêm tất cả lịch vào cơ sở dữ liệu
            if (schedules.Any())
            {
                await _scheduleRepository.AddSchedulesAsync(schedules);
            }
        }




        public async Task UpdateScheduleAsync(UpdateScheduleRequestDTO request)
        {
            var newDays = _scheduleRepository.ParseNewDays(request.DayOfWeek);
            var existingSchedules = new List<Schedule>();

            // Lấy lịch trình theo từng ngày trong tuần
            foreach (var day in newDays)
            {
                var schedulesForDay = await _scheduleRepository.GetSchedulesByCourseIdToUpdateAsync(request.CourseId, (int)day);
                existingSchedules.AddRange(schedulesForDay);
            }

            var existingDays = _scheduleRepository.GetExistingDays(existingSchedules);
            var (daysToAdd, daysToRemove) = _scheduleRepository.CompareDays(newDays, existingDays);

            // Nếu không có sự thay đổi nào về ngày
            if (!daysToAdd.Any() && !daysToRemove.Any())
            {
                var courseDetails = await _courseRepository.GetCourseByIdAsync(request.CourseId);
                var dayCounts = new Dictionary<DayOfWeek, int>();
                var loopDate = courseDetails.StartDate;

                while (loopDate <= courseDetails.EndDate)
                {
                    if (newDays.Contains(loopDate.DayOfWeek))
                    {
                        if (!dayCounts.ContainsKey(loopDate.DayOfWeek))
                        {
                            dayCounts[loopDate.DayOfWeek] = 0;
                        }
                        dayCounts[loopDate.DayOfWeek]++;
                    }
                    loopDate = loopDate.AddDays(1);
                }

                foreach (var day in newDays)
                {
                    var actualCount = dayCounts[day];
                    var existingCount = await _scheduleRepository.GetScheduleCountByDayOfWeekAsync(request.CourseId, day);

                    if (existingCount < actualCount)
                    {
                        var missingDaysCount = actualCount - existingCount;
                        loopDate = courseDetails.StartDate;

                        while (missingDaysCount > 0 && loopDate <= courseDetails.EndDate)
                        {
                            if (_scheduleRepository.IsScheduleDateValid(existingSchedules, day, loopDate))
                            {
                                var newSchedule = new Schedule
                                {
                                    CourseId = request.CourseId,
                                    ScheduleDate = loopDate,
                                    StartTime = request.StartTime,
                                    EndTime = request.EndTime,
                                    ClassroomId = request.ClassroomId
                                };
                                await _scheduleRepository.AddScheduleAsync(newSchedule);
                                missingDaysCount--;
                            }
                            loopDate = loopDate.AddDays(1);
                        }
                    }
                    else if (existingCount > actualCount)
                    {
                        var extraSchedules = _scheduleRepository.GetExtraSchedules(existingSchedules, day, existingCount - actualCount);
                        var idsToRemove = _scheduleRepository.ExtractScheduleIds(extraSchedules);

                        // Xóa Attendance liên quan đến các Schedule sẽ bị xóa
                        await _attendanceRepository.RemoveAttendancesByScheduleIdsAsync(idsToRemove.ToList());

                        // Xóa Schedule
                        await _scheduleRepository.RemoveSchedulesByIdsAsync(idsToRemove.ToList());
                    }
                }
                return;
            }

            // Xóa Attendance theo ScheduleId trước khi xóa Schedule
            var schedulesToRemove = _scheduleRepository.FilterSchedulesByDays(existingSchedules, daysToRemove);
            var removeIds = _scheduleRepository.ExtractScheduleIds(schedulesToRemove);

            // Xóa Attendance
            await _attendanceRepository.RemoveAttendancesByScheduleIdsAsync(removeIds.ToList());

            // Xóa Schedule
            await _scheduleRepository.RemoveSchedulesByIdsAsync(removeIds.ToList());

            var schedulesToUpdate = _scheduleRepository.FilterSchedulesByDays(existingSchedules, newDays);
            foreach (var schedule in schedulesToUpdate)
            {
                schedule.StartTime = request.StartTime;
                schedule.EndTime = request.EndTime;
                schedule.ClassroomId = request.ClassroomId;
            }
            await _scheduleRepository.UpdateSchedulesAsync(schedulesToUpdate);

            var course = await _courseRepository.GetCourseByIdAsync(request.CourseId);
            var newDate = course.StartDate;

            while (newDate <= course.EndDate)
            {
                if (daysToAdd.Contains(newDate.DayOfWeek))
                {
                    var newSchedule = new Schedule
                    {
                        CourseId = request.CourseId,
                        ScheduleDate = newDate,
                        StartTime = request.StartTime,
                        EndTime = request.EndTime,
                        ClassroomId = request.ClassroomId
                    };
                    await _scheduleRepository.AddScheduleAsync(newSchedule);
                }
                newDate = newDate.AddDays(1);
            }
        }


        public async Task<List<TeacherScheduleDTO>> GetTeachingScheduleAsync(int userId)
        {
            try
            {
                var teacher = await _teacherRepository.GetTeacherByUserIdAsync(userId);
                if (teacher == null)
                {
                    Console.WriteLine($"No schedules found for userId: {userId}");
                }
                var schedules = await _scheduleRepository.GetTeachingScheduleByTeacherIdAsync(teacher.TeacherId);
                if (schedules == null || !schedules.Any())
                {
                    Console.WriteLine($"No schedules found for teacherId: {teacher.TeacherId}");
                }

                return schedules;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTeachingScheduleAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Schedule>> GetScheduleByCourseIdAsync(int courseId)
        {
            try
            {
                var schedules = await _scheduleRepository.GetSchedulesByCourseIdAsync(courseId);
                if (schedules == null || !schedules.Any())
                {
                    Console.WriteLine($"No schedules found for courseId: {courseId}");
                }

                return schedules;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetScheduleByCourseIdAsync: {ex.Message}");
                throw;
            }
        }
        public async Task DeleteSchedulesByCourseIdAsync(int courseId)
        {
            // Gọi phương thức từ repository để xóa các lịch học theo CourseId
            await _scheduleRepository.DeleteSchedulesByCourseIdAsync(courseId);
        }

        

    }


}
