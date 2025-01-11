using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class CourseRepository : ICourseRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public CourseRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(course => course.Teacher) // Bao gồm mối quan hệ Teacher để lấy FirstName và LastName
                .Select(course => new CourseDTO
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    Description = course.Description,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate,
                    Price = course.Price,
                    ViewCount = course.ViewCount,
                    ImageUrl = course.ImageUrl,
                    TeacherId = course.TeacherId,
                    // Kết hợp FirstName và LastName của Teacher để tạo TeacherName
                    TeacherName = course.Teacher != null ? $"{course.Teacher.FirstName} {course.Teacher.LastName}" : null,
                })
                .ToListAsync();
        }


        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.CourseId == id);
        }

        public async Task<IEnumerable<Course>> GetCoursesByUserIdAsync(int studentId)
        {
            
            var registrations = await _context.Registrations
                                            .Where(r => r.StudentId == studentId)  
                                            .Include(r => r.Course)                
                                            .ToListAsync();
            return registrations.Select(r => r.Course);
        }


        public async Task<List<GetCourseByTeacherDTO>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            // Truy vấn dữ liệu cơ bản từ cơ sở dữ liệu
            var courses = await _context.Courses
                .Where(c => c.TeacherId == teacherId)
                .Include(c => c.Teacher)  // Bao gồm thông tin giáo viên
                .Include(c => c.Schedules)
                .ThenInclude(s => s.Classroom)  // Bao gồm thông tin Classroom
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseName,
                    c.Description,
                    c.Price,
                    c.StartDate,
                    c.EndDate,
                    c.ViewCount,
                    c.ImageUrl,
                    TeacherName = $"{c.Teacher.FirstName} {c.Teacher.LastName}",  // Tên giáo viên
                    Schedules = c.Schedules.Select(s => new
                    {
                        s.ScheduleDate,
                        s.StartTime,
                        s.EndTime,
                        s.Classroom.RoomNumber // Thông tin phòng học
                    }).ToList()
                })
                .ToListAsync();

            // Xử lý và ánh xạ dữ liệu sang DTO cho response
            var result = courses.Select(c => new GetCourseByTeacherDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                Price = c.Price,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ViewCount = c.ViewCount,
                ImageUrl = c.ImageUrl,
                TeacherName = c.TeacherName,

                // Lấy tất cả các ngày trong tuần, loại bỏ trùng lặp và sắp xếp theo thứ tự trong tuần
                DayOfWeek = string.Join(", ",
                    c.Schedules
                        .Select(s => s.ScheduleDate.DayOfWeek.ToString())  // Lấy danh sách các ngày trong tuần từ ScheduleDate
                        .Distinct()  // Loại bỏ ngày trùng
                        .OrderBy(d => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList().IndexOf(Enum.Parse<DayOfWeek>(d)))  // Sắp xếp theo thứ tự trong tuần
                ),

                // Lấy thời gian bắt đầu, kết thúc và phòng học đầu tiên (nếu có)
                StartTime = c.Schedules.FirstOrDefault()?.StartTime,  // Dùng null-conditional operator (?.) để tránh null reference
                EndTime = c.Schedules.FirstOrDefault()?.EndTime,  // Dùng null-conditional operator (?.) để tránh null reference
                RoomNumber = c.Schedules.FirstOrDefault()?.RoomNumber  // Dùng null-conditional operator (?.) để tránh null reference
            })
            .ToList();

            return result;
        }


        public async Task<Course> AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Course>> GetFeaturedCoursesAsync()
        {
            return await _context.Courses
                                 .OrderByDescending(c => c.ViewCount) // Sắp xếp theo lượt truy cập giảm dần
                                 .Take(5)                             // Lấy 5 khóa học
                                 .Include(c => c.Teacher)             // Bao gồm thông tin giảng viên nếu cần
                                 .ToListAsync();
        }
        public async Task<IEnumerable<Registration>> GetCourseMembersAsync(int courseId)
        {
            // Lấy danh sách các đăng ký của khóa học với trạng thái "Active"
            var registrations = await _context.Registrations
                                            .Include(r => r.Student) // Bao gồm thông tin sinh viên
                                            .Where(r => r.CourseId == courseId && r.Status == "Active")
                                            .ToListAsync();

            return registrations.Any() ? registrations : Enumerable.Empty<Registration>();
        }


        public async Task<List<GetCourseByStudentDTO>> GetCoursesByStudentIdAsync(int studentId)
        {
            // Truy vấn dữ liệu cơ bản từ cơ sở dữ liệu
            var courses = await _context.Courses
                .Where(c => c.Registrations.Any(r => r.StudentId == studentId))
                .Include(c => c.Teacher)
                .Include(c => c.Schedules)
                .ThenInclude(s => s.Classroom)  // Đảm bảo bao gồm cả Classroom
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseName,
                    c.Description,
                    c.Price,
                    c.StartDate,
                    c.EndDate,
                    c.ViewCount,
                    c.ImageUrl,
                    TeacherName = $"{c.Teacher.FirstName} {c.Teacher.LastName}",
                    Schedules = c.Schedules.Select(s => new
                    {
                        s.ScheduleDate,
                        s.StartTime,
                        s.EndTime,
                        s.Classroom.RoomNumber // Giữ lại thông tin Classroom
                    }).ToList()
                })
                .ToListAsync();

            // Xử lý phần lấy các ngày trong tuần sau khi dữ liệu đã được tải về
            var result = courses.Select(c => new GetCourseByStudentDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                Price = c.Price,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ViewCount = c.ViewCount,
                ImageUrl = c.ImageUrl,
                TeacherName = c.TeacherName,
                
                // Lấy tất cả các ngày trong tuần, loại bỏ trùng lặp và sắp xếp theo thứ tự trong tuần
                DayOfWeek = string.Join(", ",
                    c.Schedules
                        .Select(s => s.ScheduleDate.DayOfWeek.ToString())  // Lấy danh sách các ngày trong tuần từ ScheduleDate
                        .Distinct()  // Loại bỏ ngày trùng
                        .OrderBy(d => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList().IndexOf(Enum.Parse<DayOfWeek>(d)))  // Sắp xếp theo thứ tự trong tuần
                ),

                // Lấy thời gian bắt đầu, kết thúc và phòng học đầu tiên (nếu có)
                StartTime = c.Schedules.FirstOrDefault()?.StartTime,  // Dùng null-conditional operator (?.) để tránh null reference
                EndTime = c.Schedules.FirstOrDefault()?.EndTime,  // Dùng null-conditional operator (?.) để tránh null reference
                RoomNumber = c.Schedules.FirstOrDefault()?.RoomNumber  // Dùng null-conditional operator (?.) để tránh null reference
            })
            .ToList();

            return result;
        }

        public async Task<List<GetCourseByTeacherDTO>> GetAllCoursesInScheduleFormAsync()
        {
            // Truy vấn dữ liệu từ bảng Courses và các liên kết với Teacher, Schedules, Classroom
            var courses = await _context.Courses
                .Include(c => c.Teacher)  // Bao gồm thông tin giáo viên
                .Include(c => c.Schedules) // Bao gồm thông tin lịch học
                .ThenInclude(s => s.Classroom) // Bao gồm thông tin phòng học
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseName,
                    c.Description,
                    c.Price,
                    c.StartDate,
                    c.EndDate,
                    c.ViewCount,
                    c.ImageUrl,
                    TeacherName = $"{c.Teacher.FirstName} {c.Teacher.LastName}", // Tên giáo viên
                    Schedules = c.Schedules.Select(s => new 
                    {
                        s.ScheduleDate, 
                        s.StartTime, 
                        s.EndTime, 
                        ClassroomRoomNumber = s.Classroom.RoomNumber // Thông tin phòng học
                    }).ToList() 
                })
                .ToListAsync();

            // Xử lý dữ liệu và ánh xạ sang DTO để trả về
            var result = courses.Select(c => new GetCourseByTeacherDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                Price = c.Price,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ViewCount = c.ViewCount,
                ImageUrl = c.ImageUrl,
                TeacherName = c.TeacherName,

                // Lấy tất cả các ngày trong tuần, loại bỏ trùng lặp và sắp xếp theo thứ tự trong tuần
                DayOfWeek = string.Join(", ",
                    c.Schedules
                        .Select(s => s.ScheduleDate.DayOfWeek.ToString()) // Lấy danh sách các ngày trong tuần từ ScheduleDate
                        .Distinct()  // Loại bỏ ngày trùng
                        .OrderBy(d => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList().IndexOf(Enum.Parse<DayOfWeek>(d))) // Sắp xếp theo thứ tự trong tuần
                ),

                // Lấy thời gian bắt đầu, kết thúc và phòng học đầu tiên (nếu có)
                StartTime = c.Schedules.FirstOrDefault()?.StartTime, // Dùng null-conditional operator (?.) để tránh null reference
                EndTime = c.Schedules.FirstOrDefault()?.EndTime, // Dùng null-conditional operator (?.) để tránh null reference
                RoomNumber = c.Schedules.FirstOrDefault()?.ClassroomRoomNumber // Sử dụng ClassroomRoomNumber thay vì Classroom
            })
            .ToList();

            return result;
        }

        public async Task<GetCourseByTeacherWithClassRoomIdDTO> GetCoursesInScheduleFormWithClassIdByIdAsync(int courseId)
{
    // Truy vấn dữ liệu từ bảng Courses và các liên kết với Teacher, Schedules, Classroom
    var course = await _context.Courses
        .Include(c => c.Teacher)  // Bao gồm thông tin giáo viên
        .Include(c => c.Schedules) // Bao gồm thông tin lịch học
        .ThenInclude(s => s.Classroom) // Bao gồm thông tin phòng học
        .Where(c => c.CourseId == courseId) // Lọc theo courseId
        .Select(c => new
        {
            c.CourseId,
            c.CourseName,
            c.Description,
            c.Price,
            c.StartDate,
            c.EndDate,
            c.ViewCount,
            c.ImageUrl,
            TeacherName = $"{c.Teacher.FirstName} {c.Teacher.LastName}", // Tên giáo viên
            Schedules = c.Schedules.Select(s => new 
            {
                s.ScheduleDate, 
                s.StartTime, 
                s.EndTime, 
                ClassroomRoomNumber = s.Classroom.RoomNumber, // Thông tin phòng học
                ClassroomId = s.Classroom.ClassroomId // Lấy ClassId từ Classroom
            }).ToList() 
        })
        .FirstOrDefaultAsync(); // Chỉ lấy 1 khóa học theo courseId

    // Nếu không tìm thấy khóa học, trả về null
    if (course == null)
    {
        return null;
    }

    // Xử lý dữ liệu và ánh xạ sang DTO để trả về
    var result = new GetCourseByTeacherWithClassRoomIdDTO
    {
        CourseId = course.CourseId,
        CourseName = course.CourseName,
        Description = course.Description,
        Price = course.Price,
        StartDate = course.StartDate,
        EndDate = course.EndDate,
        ViewCount = course.ViewCount,
        ImageUrl = course.ImageUrl,
        TeacherName = course.TeacherName,

        // Lấy tất cả các ngày trong tuần, loại bỏ trùng lặp và sắp xếp theo thứ tự trong tuần
        DayOfWeek = string.Join(", ",
            course.Schedules
                .Select(s => s.ScheduleDate.DayOfWeek.ToString()) // Lấy danh sách các ngày trong tuần từ ScheduleDate
                .Distinct()  // Loại bỏ ngày trùng
                .OrderBy(d => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList().IndexOf(Enum.Parse<DayOfWeek>(d))) // Sắp xếp theo thứ tự trong tuần
        ),

        // Lấy thời gian bắt đầu, kết thúc và phòng học đầu tiên (nếu có)
        StartTime = course.Schedules.FirstOrDefault()?.StartTime, // Dùng null-conditional operator (?.) để tránh null reference
        EndTime = course.Schedules.FirstOrDefault()?.EndTime, // Dùng null-conditional operator (?.) để tránh null reference
        RoomNumber = course.Schedules.FirstOrDefault()?.ClassroomRoomNumber, // Sử dụng ClassroomRoomNumber thay vì Classroom

        // Thêm thông tin ClassId từ Classroom
        ClassroomId = course.Schedules.FirstOrDefault()?.ClassroomId // Lấy ClassId từ Classroom
    };

    return result;
}




    }
}
