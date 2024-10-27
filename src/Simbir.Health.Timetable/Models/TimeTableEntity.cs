using System.ComponentModel.DataAnnotations.Schema;

namespace Simbir.Health.Timetable.Models
{
    public class TimeTableEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("hospital_id")]
        public int HospitalId { get; set; }
        [Column("doctor_id")]
        public int DoctorId { get; set; }
        [Column("start_time")]
        public DateTimeOffset StartTime { get; set; }
        [Column("end_time")]
        public DateTimeOffset EndTime { get; set; }
        [Column("room_id")]
        public int RoomId { get; set; }
    }
}