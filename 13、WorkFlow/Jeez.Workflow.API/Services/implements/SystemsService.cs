using AutoMapper;
using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Dtos.Systems;
using Jeez.Workflow.API.Models.Systems;
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
        public async Task<CommonResult<SystemsDto>> SystemsCreateAsync(SystemsCreateDto systemsCreateDto)
        {
            CommonResult<SystemsDto> result = new CommonResult<SystemsDto>();
            Systems systems = Mapper.Map<Systems>(systemsCreateDto);
            SystemsDto systemsDto = Mapper.Map<SystemsDto>(systemsCreateDto);
            var r = await WorkflowFixtrue.Db.Systems.InsertAsync(systems);
            if (r)
            {
                result.Success = true;
                result.Message = "创建成功";
                result.Data = systemsDto;
            }
            else
            {
                result.Success = false;
                result.Message = "创建失败";
                result.Data = null;
            }
            return result;
        }

        /// <summary>
        /// 子系统模型集合查询
        /// </summary>
        /// <param name="systemsGetListDto"></param>
        /// <returns></returns>
        public async Task<CommonResult<List<SystemsDto>>> SystemsGetListAsync(SystemsGetListDto systemsGetListDto)
        {
            var result = new CommonResult<List<SystemsDto>>();
            try
            {
                IEnumerable<Systems> list = await WorkflowFixtrue.Db.Systems.FindAllAsync(u => u.IsDel == systemsGetListDto.IsDel);
                List<SystemsDto> systemList = Mapper.Map<List<SystemsDto>>(list);
                return new CommonResult<List<SystemsDto>>
                {
                    Success = true,
                    Message = "查询成功",
                    Data = systemList
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 子系统模型集合分页查询
        /// </summary>
        /// <param name="systemsGetListPageDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommonPageResult<SystemsDto>> SystemsGetListPageAsync(SystemsGetListPageDto systemsGetListPageDto)
        {
            var data = await WorkflowFixtrue.Db.Systems.SystemsGetListPageAsync(systemsGetListPageDto);
            var systemList = Mapper.Map<List<SystemsDto>>(data);
            var total = await WorkflowFixtrue.Db.Systems.SystemsGetListCountPageAsync(systemsGetListPageDto);
            return new CommonPageResult<SystemsDto>(systemsGetListPageDto.PageSize, total, systemList);
        }
    }
}
