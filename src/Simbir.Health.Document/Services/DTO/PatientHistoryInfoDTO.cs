namespace Simbir.Health.Document.Services.DTO
{
    public class PatientHistoryInfoDTO
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public int HospitalId { get; set; }
        public int DoctorId { get; set; }
        public int RoomId { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
