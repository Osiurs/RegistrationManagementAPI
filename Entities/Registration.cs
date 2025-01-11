using System.Text.Json.Serialization;
namespace RegistrationManagementAPI.Entities
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; } // "Pending", "Confirmed", "Canceled"
        
        // Relationships
         [JsonIgnore]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}