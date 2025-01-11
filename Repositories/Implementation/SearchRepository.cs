using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class SearchRepository : ISearchRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public SearchRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> SearchStudentsAsync(string query)
        {
            return await _context.Students
                .Where(s => EF.Functions.Like(s.FirstName, $"%{query}%") ||
                            EF.Functions.Like(s.LastName, $"%{query}%") ||
                            EF.Functions.Like(s.Email, $"%{query}%") ||
                            EF.Functions.Like(s.PhoneNumber, $"%{query}%") ||
                            EF.Functions.Like(s.Address, $"%{query}%") ||
                            EF.Functions.Like(s.ParentName, $"%{query}%") ||
                            EF.Functions.Like(s.ParentPhoneNumber, $"%{query}%"))
                .ToListAsync();
        }


        public async Task<IEnumerable<Course>> SearchCoursesAsync(string query)
        {
            return await _context.Courses
                .Where(c => EF.Functions.Like(c.CourseName, $"%{query}%") ||
                            EF.Functions.Like(c.Description, $"%{query}%"))
                .ToListAsync();
        }

        public async Task<IEnumerable<Teacher>> SearchTeachersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));
            }

            return await _context.Teachers
                .Where(t => EF.Functions.Like(t.FirstName, $"%{query}%") ||
                            EF.Functions.Like(t.LastName, $"%{query}%"))
                .ToListAsync();
        }

    }
}
