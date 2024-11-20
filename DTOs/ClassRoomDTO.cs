namespace RegistrationManagementAPI.DTOs
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public string Equipment { get; set; }  // Ví dụ: "Máy lạnh, Máy chiếu"
    }
}
