namespace RegistrationManagementAPI.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relationships
        public ICollection<Course> Courses { get; set; } // Tổ - bộ môn có thể liên kết với nhiều khóa học
    }
}
