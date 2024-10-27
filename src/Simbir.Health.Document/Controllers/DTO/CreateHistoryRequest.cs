using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Document.Controllers.DTO
{
    public class CreateHistoryRequest
    {
        [Required]
        public DateTimeOffset Date { get; set; }
        [Required]
        public int PacientId { get; set; }
        [Required]
        public int HospitalId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public string Room { get; set; } = string.Empty;
        [Required]
        public string Data { get; set; } = string.Empty;
    }
}
