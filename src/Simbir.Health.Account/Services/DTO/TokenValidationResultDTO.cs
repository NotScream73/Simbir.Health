using AutoMapper;
using Simbir.Health.Account.Models.DTO;

namespace Simbir.Health.Account.Services.DTO;

public class TokenValidationResultDTO
{
    public bool IsValid { get; set; }
    public int? UserId { get; set; }
    public string[]? Roles { get; set; }
}

public class TokenValidationResultProfile : Profile
{
    public TokenValidationResultProfile()
    {
        CreateMap<TokenValidationResultDTO, ValidationTokenResult>();
    }
}