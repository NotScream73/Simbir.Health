using Simbir.Health.Hospital.Controllers.DTO;
using Simbir.Health.Hospital.Services.DTO;

namespace Simbir.Health.Hospital.Services
{
    public interface IHospitalService
    {
        Task<HospitalInfoListDTO> GetHospitalsAsync(int from, int count);
        Task<HospitalDetailedInfoDTO> GetHospitalByIdAsync(int id);
        Task<List<RoomInfoDTO>> GetHospitalRoomsAsync(int hospitalId);
        Task<int> CreateHospitalAsync(HospitalCreateRequest request);
        Task UpdateHospitalAsync(int id, HospitalUpdateRequest request);
        Task DeleteHospitalByIdAsync(int hospitalId);
    }
}
