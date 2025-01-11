using Microsoft.EntityFrameworkCore;
using RegistrationManagementAPI.Entities;

namespace RegistrationManagementAPI.Data
{
        public class NVHTNQ10DbContext : DbContext
    {
        public NVHTNQ10DbContext(DbContextOptions<NVHTNQ10DbContext> options) : base(options) {
            
         }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReadStatus> MessageReadStatus { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<TuitionFee> TuitionFees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MessageReadStatus>()
            .Property(m => m.MessageReadStatusId)
            .ValueGeneratedOnAdd();  // Ensures that the value is automatically generated
            modelBuilder.Entity<Attendance>().ToTable("Attendance");
             modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId);
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Schedule)
                .WithMany()
                .HasForeignKey(a => a.ScheduleId);
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId);
                // Đảm bảo không có cấu hình TeacherId
                entity.Property(e => e.ClassroomId).IsRequired();
                entity.Property(e => e.CourseId).IsRequired();
                entity.Property(e => e.ScheduleDate).IsRequired();
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
            });
            // Cấu hình cho Teacher
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Courses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId);

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Salaries)
                .WithOne(s => s.Teacher)
                .HasForeignKey(s => s.TeacherId);

            // Cấu hình cho Student
            modelBuilder.Entity<Student>()
                .HasMany(s => s.TuitionFees)
                .WithOne(tf => tf.Student)
                .HasForeignKey(tf => tf.StudentId);

            // Chỉ định khóa chính cho TuitionFee
        modelBuilder.Entity<TuitionFee>()
            .HasKey(t => t.TuitionId);  
        }


    }
}