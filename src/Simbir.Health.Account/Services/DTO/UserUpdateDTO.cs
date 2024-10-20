using AutoMapper;
using Simbir.Health.Account.Models;
using Simbir.Health.Account.Models.DTO;

namespace Simbir.Health.Account.Services.DTO;

public class UserUpdateDTO
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Password { get; set; }
}

public class UserUpdateByAdminDTO : UserUpdateDTO
{
    public string Username { get; set; }
    public string[] Roles { get; set; }
}

public class UserUpdateProfile : Profile
{
    public UserUpdateProfile()
    {
        CreateMap<AccountUpdateRequest, UserUpdateDTO>();

        CreateMap<UserUpdateDTO, User>()
            .ForMember(entity => entity.Id, opt => opt.Ignore())
            .ForMember(entity => entity.UserName, opt => opt.Ignore())
            .ForMember(entity => entity.PasswordHash, opt => opt.Ignore())
            .ForMember(entity => entity.IsLogin, opt => opt.Ignore())
            .ForMember(entity => entity.IsDeleted, opt => opt.Ignore())
            .ForMember(entity => entity.UserRoles, opt => opt.Ignore());

        CreateMap<UpdateUserRequest, UserUpdateByAdminDTO>();

        CreateMap<UserUpdateByAdminDTO, User>()
            .ForMember(entity => entity.Id, opt => opt.Ignore())
            .ForMember(entity => entity.PasswordHash, opt => opt.Ignore())
            .ForMember(entity => entity.IsLogin, opt => opt.Ignore())
            .ForMember(entity => entity.IsDeleted, opt => opt.Ignore())
            .ForMember(entity => entity.UserRoles, opt => opt.Ignore());
    }
}