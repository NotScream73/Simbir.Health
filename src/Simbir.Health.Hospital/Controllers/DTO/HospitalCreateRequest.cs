using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Hospital.Controllers.DTO;

public class HospitalCreateRequest
{
    [Required]
    [StringLength(256, ErrorMessage = "Название больницы должно быть не больше 256 символов в длину")]
    public string Name { get; set; }
    [Required]
    [StringLength(256, ErrorMessage = "Адрес должен быть не больше 256 символов в длину")]
    public string Address { get; set; }
    [Required]
    [StringLength(256, ErrorMessage = "Номер телефона должен быть не больше 256 символов в длину")]
    public string ContactPhone { get; set; }
    [Required]
    public List<string> Rooms { get; set; } = new List<string>();
}