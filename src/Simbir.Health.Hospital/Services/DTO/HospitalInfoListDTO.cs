namespace Simbir.Health.Hospital.Services.DTO
{
    public class HospitalInfoListDTO
    {
        public List<HospitalInfoDTO> Hospitals { get; set; } = [];
        public int TotalCount { get; set; }
    }
}
