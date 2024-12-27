using AutoMapper;
using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Models.DataPrivilege;

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
            CreateMap<User, UserDto>();
            CreateMap<UserCreateDto, DataPrivilege>()
                // 忽略Id属性，因为DataPrivilege的Id是主键，通常由数据库自动生成
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SystemId, opt => opt.MapFrom(src => src.SystemId))
                // 假设UserCreateDto中没有DeptId属性，所以忽略
                .ForMember(dest => dest.DeptId, opt => opt.Ignore()); 

            CreateMap<DeptCreateDto, Dept>();
            CreateMap<Dept, DeptDto>();
            CreateMap<DeptUpdateDto, Dept>();
        }
    }
}
