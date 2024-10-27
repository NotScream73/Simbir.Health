using Simbir.Health.Document.Services.DTO;

namespace Simbir.Health.Document.Services
{
    public interface IHistoryService
    {
        Task CreateHistoryAsync(CreateHistoryDTO historyDTO, string accessToken);
        Task UpdateHistoryAsync(int id, UpdateHistoryDTO historyDTO, string accessToken);
        Task<IEnumerable<PatientHistoryInfoDTO>> GetPatientHistoryAsync(int id, string accessToken);
        Task<GetHistoryInfoDTO> GetHistoryAsync(int id, string accessToken);
    }
}
