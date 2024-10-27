using System.ComponentModel.DataAnnotations.Schema;

namespace Simbir.Health.Hospital.Models
{
    public class Room
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("hospital_id")]
        public int HospitalId { get; set; }
    }
}
