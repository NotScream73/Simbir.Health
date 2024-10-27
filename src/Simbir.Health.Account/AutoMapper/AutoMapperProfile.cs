using AutoMapper;
using Simbir.Health.Account.Controllers.DTO;
using Simbir.Health.Account.Models;
using Simbir.Health.Account.Services.DTO;

namespace Simbir.Health.Account.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccessAndRefreshTokenDTO, SignInResponse>();
            CreateMap<AccessAndRefreshTokenDTO, RefreshTokenResponse>();
            CreateMap<AccountMeInfoDTO, AccountMeInfoResponse>();
            CreateMap<User, DoctorInformationDTO>();
            CreateMap<TokenValidationResultDTO, ValidationTokenResult>();

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
}
