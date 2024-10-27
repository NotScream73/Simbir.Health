using System.ComponentModel.DataAnnotations.Schema;

namespace Simbir.Health.Timetable.Models
{
    public class Appointment
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("time_table_id")]
        public int TimetableId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("reception_time")]
        public DateTimeOffset ReceptionTime { get; set; }
    }
}
