using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public CourseService(ICourseRepository courseRepository, 
                            ITeacherRepository teacherRepository, 
                            IStudentRepository studentRepository,
                            IRegistrationRepository registrationRepository,
                            IMessageRepository messageRepository,
                            IPaymentRepository paymentRepository,
                            IScheduleRepository scheduleRepository)
        {
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _registrationRepository = registrationRepository;
            _messageRepository = messageRepository;
            _paymentRepository = paymentRepository;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllCoursesAsync();
        }

        public async Task<CourseDTO> GetCourseByIdAsync(int id)
    {
        var course = await _courseRepository.GetCourseByIdAsync(id);
        if (course == null)
        {
            throw new InvalidOperationException("Course not found.");
        }
        var teacher = await _teacherRepository.GetTeacherByTeacherIdAsync(course.TeacherId);
        if (teacher == null)
        {
            throw new InvalidOperationException("Teacher not found.");
        }
        string showDescription;
        if (course.Description != null)
        {
            showDescription = course.Description;
        }
        else  showDescription = "No description available";

        return new CourseDTO
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = showDescription,
                Price = course.Price,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                TeacherId = course.TeacherId,
                ImageUrl = course.ImageUrl,
                TeacherName = teacher.FirstName +" " + teacher.LastName,
            };
    }

        public async Task<IEnumerable<GetCourseByStudentDTO>> GetCoursesByUserIdAsync(int UserId)
        {
            var student = await _studentRepository.GetStudentByUserIdAsync(UserId);
            var teacher = await _teacherRepository.GetTeacherByUserIdAsync(UserId);
            IEnumerable<GetCourseByStudentDTO> courses = null;

            if (student != null)
            {
                courses = await _courseRepository.GetCoursesByStudentIdAsync(student.StudentId);
            }
            else if (teacher != null)
            {
                // Nếu muốn xử lý thêm với teacher thì thêm logic tại đây
                throw new NotImplementedException("Teacher-specific logic not implemented.");
            }

            return courses;
        }



        public async Task<IEnumerable<GetCourseByTeacherDTO>> GetCoursesAsTeacherAsync(int userId)
        {
            // Lấy thông tin giáo viên từ UserId
            var teacher = await _teacherRepository.GetTeacherByUserIdAsync(userId);
            
            if (teacher == null)
                throw new Exception($"Teacher with ID {userId} does not exist");

            // Gọi hàm để lấy thông tin khóa học của giáo viên
            var courses = await _courseRepository.GetCoursesByTeacherIdAsync(teacher.TeacherId);
            return courses.Select(course => new GetCourseByTeacherDTO
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                TeacherName = course.TeacherName,  // Thêm tên giáo viên
                DayOfWeek = course.DayOfWeek,      // Thêm các ngày trong tuần
                StartTime = course.StartTime,      // Thêm thời gian bắt đầu
                EndTime = course.EndTime,          // Thêm thời gian kết thúc
                RoomNumber = course.RoomNumber     // Thêm số phòng học
            });
        }


        public async Task<CourseDTO> AddCourseAsync(CourseDTO courseDto)
        {
            if (courseDto == null)
                throw new ArgumentNullException(nameof(courseDto), "Course data cannot be null");

            // Kiểm tra xem TeacherId có tồn tại hay không
            var teacher = await _teacherRepository.GetTeacherByTeacherIdAsync(courseDto.TeacherId);
            if (teacher == null)
                throw new Exception($"Teacher with ID {courseDto.TeacherId} does not exist");

            var course = new Course
            {
                CourseName = courseDto.CourseName,
                Price = courseDto.Price,
                StartDate = courseDto.StartDate,
                EndDate = courseDto.EndDate,
                TeacherId = courseDto.TeacherId,
                Description = courseDto.Description,
                ImageUrl = courseDto.ImageUrl
            };

            var createdCourse = await _courseRepository.AddCourseAsync(course);

            return new CourseDTO
            {
                CourseId = createdCourse.CourseId,
                CourseName = createdCourse.CourseName,
                Price = createdCourse.Price,
                StartDate = createdCourse.StartDate,
                EndDate = createdCourse.EndDate,              
                TeacherId = createdCourse.TeacherId,
                Description = createdCourse.Description,
                ImageUrl = createdCourse.ImageUrl                
            };
        }


                public async Task<CourseDTO> UpdateCourseAsync(int id, CourseDTO courseDto)
                {
                    var existingCourse = await _courseRepository.GetCourseByIdAsync(id);
                    if (existingCourse == null)
                    {
                        throw new InvalidOperationException("Course not found.");
                    }

                    existingCourse.CourseName = courseDto.CourseName;
                    existingCourse.Description = courseDto.Description;
                    existingCourse.StartDate = courseDto.StartDate;
                    existingCourse.EndDate = courseDto.EndDate;
                    existingCourse.Price = courseDto.Price;           
                    existingCourse.TeacherId = courseDto.TeacherId;
                    existingCourse.ImageUrl = courseDto.ImageUrl;

                    await _courseRepository.UpdateCourseAsync(existingCourse);
                    return new CourseDTO
                    {
                        CourseName = existingCourse.CourseName,
                        Price = existingCourse.Price,
                        StartDate = existingCourse.StartDate,
                        EndDate = existingCourse.EndDate,        
                        TeacherId = existingCourse.TeacherId,
                        ImageUrl = existingCourse.ImageUrl
                    };
                }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            var registrations = await _registrationRepository.GetRegistrationsByCourseIdAsync(id);
            if (registrations == null)
            {
                throw new InvalidOperationException("Registration not found.");
            }
            foreach (var registration in registrations)
            {
                await _registrationRepository.DeleteRegistrationAsync(registration.RegistrationId);
            }
            await _messageRepository.DeleteMessagesByCourseIdAsync(id);
            await _scheduleRepository.DeleteSchedulesByCourseIdAsync(id);
            await _courseRepository.DeleteCourseAsync(id);
            
        }

        public async Task<IEnumerable<CourseDTO>> GetFeaturedCoursesAsync()
        {
            var courses = await _courseRepository.GetFeaturedCoursesAsync();

            // Map từ Entity sang DTO
            return courses.Select(c => new CourseDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description ?? "No description available", // Nếu Description null
                Price = c.Price,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                TeacherId = c.TeacherId,
                ImageUrl = c.ImageUrl
            });
        }

        public async Task<Course> IncrementViewCountAsync(int courseId)
        {
            var course = await _courseRepository.GetCourseByIdAsync(courseId);

            if (course == null)
            {
                return null; // Nếu không tìm thấy khóa học
            }

            course.ViewCount += 1; // Tăng ViewCount lên 1

            return await _courseRepository.UpdateCourseAsync(course); // Cập nhật khóa học và trả về đối tượng đã cập nhật
        }
        
        public async Task<IEnumerable<Registration>> GetCourseMembersAsync(int courseId)
        {
            var courseMembers = await _courseRepository.GetCourseMembersAsync(courseId);

            if (courseMembers == null || !courseMembers.Any())
            {
                throw new InvalidOperationException("No members available at the moment.");
            }

            return courseMembers;
        }

        public async Task<List<GetCourseByStudentDTO>> GetCoursesByStudentAsync(int userId)
        {
            var student = await _studentRepository.GetStudentByUserIdAsync(userId);
            return await _courseRepository.GetCoursesByStudentIdAsync(student.StudentId);
        }

        public async Task<CourseDTO> UpdateCourseByScheduleAsync(int id, CourseDTO courseDto)
        {
            var existingCourse = await _courseRepository.GetCourseByIdAsync(id);
            if (existingCourse == null)
            {
                throw new InvalidOperationException("Course not found.");
            }
            existingCourse.CourseName = existingCourse.CourseName;
            existingCourse.Description = existingCourse.Description;
            existingCourse.StartDate = existingCourse.StartDate;
            existingCourse.EndDate = existingCourse.EndDate;
            existingCourse.Price = existingCourse.Price;           
            existingCourse.TeacherId = existingCourse.TeacherId;
            existingCourse.ImageUrl = existingCourse.ImageUrl;
            await _courseRepository.UpdateCourseAsync(existingCourse);
            return new CourseDTO
            {
                CourseName = existingCourse.CourseName,
                Price = existingCourse.Price,
                StartDate = existingCourse.StartDate,
                EndDate = existingCourse.EndDate,      
                TeacherId = existingCourse.TeacherId,
                ImageUrl = existingCourse.ImageUrl
            };
        }

        public async Task<List<GetCourseByTeacherDTO>> GetAllCoursesInScheduleFormAsync()
        {
            try
            {
                // Gọi phương thức từ repository để lấy dữ liệu khóa học
                var courses = await _courseRepository.GetAllCoursesInScheduleFormAsync();

                // Trả về kết quả như một danh sách DTO
                return courses;
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý lỗi theo yêu cầu của bạn
                throw new Exception("Error while fetching courses", ex);
            }
        }

        public async Task<GetCourseByTeacherWithClassRoomIdDTO> GetCoursesInScheduleFormByIdAsync(int courseId)
        {
            try
            {
                // Gọi phương thức từ repository để lấy dữ liệu khóa học
                var courses = await _courseRepository.GetCoursesInScheduleFormWithClassIdByIdAsync(courseId);

                // Trả về kết quả như một danh sách DTO
                return courses;
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý lỗi theo yêu cầu của bạn
                throw new Exception("Error while fetching courses", ex);
            }
        }
    }
}
