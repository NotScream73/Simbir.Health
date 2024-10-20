using AutoMapper;
using Simbir.Health.Account.Models;

namespace Simbir.Health.Account.Services.DTO;

public class DoctorInformationDTO
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}

public class DoctorInformationProfile : Profile
{
    public DoctorInformationProfile()
    {
        CreateMap<User, DoctorInformationDTO>();
    }
}