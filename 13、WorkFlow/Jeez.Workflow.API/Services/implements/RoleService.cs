using AutoMapper;
using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;

namespace Jeez.Workflow.API.Services.implements
{
    public class RoleService : IRoleService
    {
        public WorkflowFixtrue WorkflowFixtrue { get; set; }

        public IMapper Mapper { get; set; }

        public RoleService(WorkflowFixtrue workflowFixtrue, IMapper mapper)
        {
            WorkflowFixtrue = workflowFixtrue;
            Mapper = mapper;
        }

        public Task<bool> RoleCreateAsync(RoleCreateDto RoleCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<RoleDto>> RoleGetListAsync(RoleGetListDto RoleGetListDto)
        {
            throw new NotImplementedException();
        }

        public Task<RolePageDto> RoleGetListPageAsync(RoleGetListPageDto RoleGetListPageDto)
        {
            throw new NotImplementedException();
        }

        public Task<RoleDto> RoleGetAsync(long RoleId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoleUpdateAsync(RoleUpdateDto RoleUpdateDto, long RoleId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoleDeleteAsync(List<long> RoleIds)
        {
            throw new NotImplementedException();
        }
    }
}
