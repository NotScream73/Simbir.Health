namespace Simbir.Health.Timetable.Services.DTO
{
    public class RoomTimeTableFilterDTO : TimeTableFilterDTO
    {
        public int HospitalId { get; set; }
        public string RoomNumber { get; set; }
    }
}
