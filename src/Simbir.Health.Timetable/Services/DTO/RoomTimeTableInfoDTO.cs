namespace Simbir.Health.Timetable.Services.DTO
{
    public class RoomTimeTableInfoDTO
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string DoctorFullName { get; set; } = string.Empty;
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }
}
