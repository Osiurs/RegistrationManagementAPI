namespace RegistrationManagementAPI.DTOs
{
    public class ScheduleDTO
    {
        public string CourseName { get; set; }
        public DateTime StartTime { get; set; } // Thời gian bắt đầu
        public DateTime EndTime { get; set; }   // Thời gian kết thúc
        public string RoomNumber { get; set; }  // Số phòng học
        public int  TeacherId {get; set;}
        
    }
}
