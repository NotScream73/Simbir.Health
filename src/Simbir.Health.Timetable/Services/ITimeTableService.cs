using Simbir.Health.Timetable.Models.DTO;
using Simbir.Health.Timetable.Services.DTO;

namespace Simbir.Health.Timetable.Services
{
    public interface ITimeTableService
    {
        Task CreateTimeTableAsync(TimeTableCreateRequest request, string accessToken);
        Task DeleteDoctorTimeTableAsync(int doctorId);
        Task DeleteHospitalTimeTableAsync(int hospitalId);
        Task DeleteTimeTableAsync(int timeTableId);
        Task<List<DoctorTimeTableInfoDTO>> GetDoctorTimeTableListAsync(int doctorId, TimeTableFilterDTO filter, string accessToken);
        Task<List<HospitalTimeTableInfoDTO>> GetHospitalTimeTableListAsync(int hospitalId, TimeTableFilterDTO filter, string accessToken);
        Task<List<DateTimeOffset>> GetAvailableAppointmentsByTimeTableId(int id);
        Task<List<RoomTimeTableInfoDTO>> GetRoomTimeTableListAsync(RoomTimeTableFilterDTO filter, string accessToken);
        Task UpdateTimeTableAsync(int timeTableId, TimeTableCreateRequest request, string accessToken);
        Task DeleteAppointmentAsync(int id, string accessToken);
        Task CreateAppointmentAsync(int id, AppointmentCreateRequest request, string accessToken);
    }
}
