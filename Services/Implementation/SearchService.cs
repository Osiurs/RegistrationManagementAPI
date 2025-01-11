using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementation
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;

        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<IEnumerable<StudentSearchDTO>> SearchStudentsAsync(string query)
        {
            var students = await _searchRepository.SearchStudentsAsync(query);

            return students.Select(s => new StudentSearchDTO
            {
                StudentId = s.StudentId,
                FullName = $"{s.FirstName} {s.LastName}",
                DateOfBirth = s.DateOfBirth,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                ParentName = s.ParentName,
                ParentPhoneNumber = s.ParentPhoneNumber
            });
 

        }

        public async Task<IEnumerable<CourseSearchDTO>> SearchCoursesAsync(string query)
        {
            var courses = await _searchRepository.SearchCoursesAsync(query);

            return courses.Select(c => new CourseSearchDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ImageUrl = c.ImageUrl
            });
        }
        public async Task<IEnumerable<Teacher>> SearchTeachersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));
            }

            var teacher = await _searchRepository.SearchTeachersAsync(query);
            return teacher;
        }
    }
}
