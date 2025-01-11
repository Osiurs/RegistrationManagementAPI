using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Data;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Repositories.Interface;

namespace RegistrationManagementAPI.Repositories.Implementation
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly NVHTNQ10DbContext _context;

        public RegistrationRepository(NVHTNQ10DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
        {
            return await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Course)
                .ToListAsync();
        }

        public async Task<Registration> GetRegistrationByIdAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Course)
                .FirstOrDefaultAsync(r => r.RegistrationId == id);
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByStudentIdAsync(int studentId)
        {
            return await _context.Registrations
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Student)
                .Include(r => r.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCourseIdAsync(int courseId)
        {
            return await _context.Registrations
                .Where(r => r.CourseId == courseId)
                .Include(r => r.Student)
                .Include(r => r.Course)
                .ToListAsync();
        }

        public async Task<Registration> AddRegistrationAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task UpdateRegistrationAsync(Registration registration)
        {
            _context.Registrations.Update(registration);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRegistrationAsync(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Registration>> GetPendingRegistrationsByStudentIdAsync(int studentId)
        {
            return await _context.Registrations
                .Where(r => r.StudentId == studentId && r.Status == "Pending")
                .Include(r => r.Student)
                .Include(r => r.Course)
                .ToListAsync();
        }
        public async Task<Registration> GetPendingRegistrationsByStudentIdAndCourseIdAsync(int studentId, int courseId)
        {
            return await _context.Registrations
                .Where(r => r.StudentId == studentId && r.CourseId == courseId && r.Status == "Pending")
                .FirstOrDefaultAsync();
        }
        // Hàm lấy tất cả học sinh có Status là "Active" theo CourseId
        public async Task<IEnumerable<Student>> GetActiveStudentsByCourseIdAsync(int courseId)
        {
            return await _context.Registrations
                .Where(r => r.CourseId == courseId && r.Status == "Active")
                .Include(r => r.Student)  // Include Student data
                .Select(r => r.Student)   // Select only the Student entity
                .ToListAsync();
        }

        // Hàm lấy tất cả học sinh có Status là "Completed" theo CourseId
        public async Task<IEnumerable<Student>> GetCompletedStudentsByCourseIdAsync(int courseId)
        {
            return await _context.Registrations
                .Where(r => r.CourseId == courseId && r.Status == "Completed")
                .Include(r => r.Student)  // Include Student data
                .Select(r => r.Student)   // Select only the Student entity
                .ToListAsync();
        }

        

    }
}
