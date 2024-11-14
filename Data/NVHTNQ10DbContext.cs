using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Data
{
        public class NVHTNQ10DbContext : DbContext
    {
        public NVHTNQ10DbContext(DbContextOptions<NVHTNQ10DbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<User> Users { get; set; }
    }
}