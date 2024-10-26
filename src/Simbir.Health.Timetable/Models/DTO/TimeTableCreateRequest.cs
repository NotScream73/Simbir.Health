using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Timetable.Models.DTO
{
    public class TimeTableCreateRequest
    {
        [Required]
        public int HospitalId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public DateTimeOffset From { get; set; }
        [Required]
        public DateTimeOffset To { get; set; }
        [Required]
        public string Room { get; set; }
    }
}
