using AutoMapper;
using Simbir.Health.Account.Models.DTO;

namespace Simbir.Health.Account.Services.DTO;

public class AccessAndRefreshTokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class AccessAndRefreshTokenProfile : Profile
{
    public AccessAndRefreshTokenProfile()
    {
        CreateMap<AccessAndRefreshTokenDTO, SignInResponse>();
        CreateMap<AccessAndRefreshTokenDTO, RefreshTokenResponse>();
    }
}