namespace Simbir.Health.Document.Services.DTO
{
    public class GetHistoryInfoDTO
    {
        public DateTimeOffset Date { get; set; }
        public int PatientId { get; set; }
        public int HospitalId { get; set; }
        public int DoctorId { get; set; }
        public int RoomId { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}