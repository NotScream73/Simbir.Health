namespace Simbir.Health.Document.Services.DTO;

public abstract class HistoryDTO
{
    public DateTimeOffset Date { get; set; }
    public int PatientId { get; set; }
    public int HospitalId { get; set; }
    public int DoctorId { get; set; }
    public string Room { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}

public class CreateHistoryDTO : HistoryDTO { }

public class UpdateHistoryDTO : HistoryDTO { }