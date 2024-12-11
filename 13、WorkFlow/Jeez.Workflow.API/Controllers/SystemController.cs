using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos.Systems;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController
    {
        private ISystemsService SystemsService { get; set; }

        public SystemController(ISystemsService systemsService) 
        {
            SystemsService = systemsService;
        }

        /// <summary>
        /// 子系统模型创建
        /// </summary>
        /// <param name="systemsCreateDto"></param>
        /// <returns></returns>
        [HttpPost("SystemsCreateAsync")]
        public async Task<CommonResult> SystemsCreateAsync(SystemsCreateDto systemsCreateDto)
        {
            return await SystemsService.SystemsCreateAsync(systemsCreateDto);
        }

        /// <summary>
        /// 子系统模型集合查询
        /// </summary>
        /// <param name="systemsGetListDto"></param>
        /// <returns></returns>
        [HttpPost("SystemsGetListAsync")]
        public async Task<CommonResult<List<SystemsDto>>> SystemsGetListAsync(SystemsGetListDto systemsGetListDto)
        {
            return await SystemsService.SystemsGetListAsync(systemsGetListDto);
        }

        /// <summary>
        /// 子系统模型集合分页查询
        /// </summary>
        /// <param name="systemsGetListPageDto"></param>
        /// <returns></returns>
        [HttpPost("SystemsGetListPageAsync")]
        public async Task<CommonPageResult<SystemsDto>> SystemsGetListPageAsync(SystemsGetListPageDto systemsGetListPageDto)
        {
            return await SystemsService.SystemsGetListPageAsync(systemsGetListPageDto);
        }
    }
}
