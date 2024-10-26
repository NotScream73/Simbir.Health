namespace Simbir.Health.Timetable.Services.DTO
{
    public class DoctorTimeTableInfoDTO
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
    }
}
