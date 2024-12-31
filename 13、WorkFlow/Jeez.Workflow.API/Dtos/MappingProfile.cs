using AutoMapper;
using Jeez.Workflow.API.Models;

namespace Jeez.Workflow.API.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SystemsCreateDto, Systems>();
            CreateMap<SystemsCreateDto, SystemsDto>()
                .ForMember(System => System.SystemId, opt => opt.Ignore());
            CreateMap<Systems, SystemsCreateDto>();
            CreateMap<Systems, SystemsDto>();

            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.DepartId, opt => opt.Ignore());

            CreateMap<DeptCreateDto, Dept>();
            CreateMap<Dept, DeptDto>();
            CreateMap<DeptUpdateDto, Dept>();
        }
    }
}
