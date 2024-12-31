using AutoMapper;
using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Services.interfaces;

namespace Jeez.Workflow.API.Services.implements
{
    public class SystemsService : ISystemsService
    {
        public WorkflowFixtrue WorkflowFixtrue { get; set; }

        public IMapper Mapper { get; set; }

        public SystemsService(WorkflowFixtrue workflowFixtrue, IMapper mapper)
        {
            WorkflowFixtrue = workflowFixtrue;
            Mapper = mapper;
        }

        /// <summary>
        /// 子系统模型创建
        /// </summary>
        /// <param name="SystemsCreateDto"></param>
        /// <returns></returns>
        public async Task<CommonResult> SystemsCreateAsync(SystemsCreateDto systemsCreateDto)
        {
            Systems systems = Mapper.Map<Systems>(systemsCreateDto);
            await WorkflowFixtrue.Db.Systems.InsertAsync(systems);
            return new CommonResult(true, "创建成功");
        }

        /// <summary>
        /// 子系统模型集合查询
        /// </summary>
        /// <param name="systemsGetListDto"></param>
        /// <returns></returns>
        public async Task<CommonResult<List<SystemsDto>>> SystemsGetListAsync(SystemsGetListDto systemsGetListDto)
        {
            IEnumerable<Systems> list = await WorkflowFixtrue.Db.Systems.FindAllAsync(u => u.IsDel == systemsGetListDto.IsDel);
            List<SystemsDto> systemList = Mapper.Map<List<SystemsDto>>(list);
            return new CommonResult<List<SystemsDto>>(true, "查询成功", systemList);
        }

        /// <summary>
        /// 子系统模型集合分页查询
        /// </summary>
        /// <param name="systemsGetListPageDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CommonPageResult<SystemsDto>> SystemsGetListPageAsync(SystemsGetListPageDto systemsGetListPageDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SystemId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CommonResult<SystemsDto>> SystemsGetAsync(long SystemId)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult> SystemsUpdateAsync(SystemsUpdateDto SystemsUpdateDto, long SystemId)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult> SystemsDeleteAsync(List<long> SystemIds)
        {
            throw new NotImplementedException();
        }
    }
}
