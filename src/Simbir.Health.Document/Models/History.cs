using System.ComponentModel.DataAnnotations.Schema;

namespace Simbir.Health.Document.Models
{
    public class History
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("date")]
        public DateTimeOffset Date { get; set; }
        [Column("patient_id")]
        public int PatientId { get; set; }
        [Column("hospital_id")]
        public int HospitalId { get; set; }
        [Column("doctor_id")]
        public int DoctorId { get; set; }
        [Column("room_id")]
        public int RoomId { get; set; }
        [Column("data")]
        public string Data { get; set; } = string.Empty;
    }
}
