namespace RegistrationManagementAPI.DTOs
{
    public class ClassroomDTO
    {
        public int ClassroomId { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public string Equipment { get; set; }
        public List<ScheduleDTO>? Schedules { get; set; }
    }
}
