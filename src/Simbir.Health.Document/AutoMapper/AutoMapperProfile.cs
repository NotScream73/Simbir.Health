using AutoMapper;
using Simbir.Health.Document.Controllers.DTO;
using Simbir.Health.Document.Models;
using Simbir.Health.Document.Services.DTO;

namespace Simbir.Health.Document.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateHistoryRequest, CreateHistoryDTO>()
                .ForMember(dto => dto.PatientId, opt => opt.MapFrom(src => src.PacientId));
            CreateMap<UpdateHistoryRequest, UpdateHistoryDTO>()
                .ForMember(dto => dto.PatientId, opt => opt.MapFrom(src => src.PacientId));
            CreateMap<History, PatientHistoryInfoDTO>();
            CreateMap<History, GetHistoryInfoDTO>();
        }
    }
}
