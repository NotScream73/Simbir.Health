using System.Text.Json.Serialization;

namespace Simbir.Health.Hospital.Controllers.DTO
{
    public class ValidationTokenResult
    {
        [JsonPropertyName("isValid")]
        public bool IsValid { get; set; }
        [JsonPropertyName("userId")]
        public int? UserId { get; set; }
        [JsonPropertyName("roles")]
        public string[]? Roles { get; set; }
    }
}
