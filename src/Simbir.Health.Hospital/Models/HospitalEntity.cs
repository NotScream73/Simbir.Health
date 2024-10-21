using System.ComponentModel.DataAnnotations.Schema;

namespace Simbir.Health.Hospital.Models
{
    public class HospitalEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("contact_phone")]
        public string ContactPhone { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}
