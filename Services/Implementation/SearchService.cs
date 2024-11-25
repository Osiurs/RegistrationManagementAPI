using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Repositories.Interface;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Services.Implementations
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
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
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
                EndDate = c.EndDate
            });
        }
    }
}
