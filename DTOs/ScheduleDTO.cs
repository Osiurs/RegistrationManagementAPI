namespace RegistrationManagementAPI.DTOs
{
    public class ScheduleDTO
    {
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; } // Thời gian bắt đầu
        public DateTime EndDate { get; set; }   // Thời gian kết thúc
        public string RoomNumber { get; set; }  // Số phòng học
    }
}
