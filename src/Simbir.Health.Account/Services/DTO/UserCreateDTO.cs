using AutoMapper;
using Simbir.Health.Account.Models;
using Simbir.Health.Account.Models.DTO;

namespace Simbir.Health.Account.Services.DTO;

public class UserCreateDTO
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class UserCreateByAdminDTO : UserCreateDTO
{
    public string[] Roles { get; set; }
}

public class UserCreateProfile : Profile
{
    public UserCreateProfile()
    {
        CreateMap<SignUpRequest, UserCreateDTO>();

        CreateMap<UserCreateDTO, User>()
            .ForMember(entity => entity.Id, opt => opt.Ignore())
            .ForMember(entity => entity.PasswordHash, opt => opt.Ignore())
            .ForMember(entity => entity.UserRoles, opt => opt.Ignore())
            .ForMember(entity => entity.IsDeleted, opt => opt.Ignore())
            .ForMember(entity => entity.IsLogin, opt => opt.Ignore());

        CreateMap<SignUpUserRequest, UserCreateByAdminDTO>();

        CreateMap<UserCreateByAdminDTO, User>()
            .ForMember(entity => entity.Id, opt => opt.Ignore())
            .ForMember(entity => entity.PasswordHash, opt => opt.Ignore())
            .ForMember(entity => entity.UserRoles, opt => opt.Ignore())
            .ForMember(entity => entity.IsDeleted, opt => opt.Ignore())
            .ForMember(entity => entity.IsLogin, opt => opt.Ignore());
    }
}