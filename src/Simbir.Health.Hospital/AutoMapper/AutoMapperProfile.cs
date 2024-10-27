using AutoMapper;
using Simbir.Health.Hospital.Controllers.DTO;
using Simbir.Health.Hospital.Models;

namespace Simbir.Health.Hospital.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<HospitalCreateRequest, HospitalEntity>()
                .ForMember(entity => entity.Id, opt => opt.Ignore())
                .ForMember(entity => entity.IsDeleted, opt => opt.Ignore());
            CreateMap<HospitalUpdateRequest, HospitalEntity>()
                .ForMember(entity => entity.Id, opt => opt.Ignore())
                .ForMember(entity => entity.IsDeleted, opt => opt.Ignore());
        }
    }
}
