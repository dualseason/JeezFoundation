using AutoMapper;
using Jeez.Workflow.API.Dtos.Systems;

namespace Jeez.Workflow.API.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SystemsCreateDto, Models.Systems.Systems>();
            CreateMap<SystemsCreateDto, SystemsDto>().ForMember(dest => dest.SystemId, opt => opt.Ignore());
            CreateMap<Models.Systems.Systems, SystemsCreateDto>();
            CreateMap<Models.Systems.Systems, SystemsDto>();
        }
    }
}
