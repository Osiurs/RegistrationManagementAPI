namespace RegistrationManagementAPI.Entities
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public string Equipment { get; set; }  // Ví dụ: "Máy lạnh, Máy chiếu"

        // Relationships
        public ICollection<Schedule> Schedules { get; set; } // Liên kết với các buổi học trong phòng này
    }
}
