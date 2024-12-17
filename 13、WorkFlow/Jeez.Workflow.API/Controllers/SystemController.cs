using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
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
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResult> SystemsCreateAsync(SystemsCreateDto SystemsCreateDto)
        {
            return await SystemsService.SystemsCreateAsync(SystemsCreateDto);
        }

        /// <summary>
        /// 子系统模型集合查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResult<List<SystemsDto>>> SystemsGetListAsync([FromQuery] SystemsGetListDto SystemsGetListDto)
        {
            return await SystemsService.SystemsGetListAsync(SystemsGetListDto);
        }

        /// <summary>
        /// 子系统模型集合分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<CommonPageResult<SystemsDto>> SystemsGetListPageAsync([FromQuery] SystemsGetListPageDto SystemsGetListPageDto)
        {
            return await SystemsService.SystemsGetListPageAsync(SystemsGetListPageDto);
        }
        /// <summary>
        /// 子系统模型查询【根据子系统模型Id查询】
        /// </summary>
        /// <returns></returns>
        [HttpGet("{SystemId}")]
        public async Task<CommonResult<SystemsDto>> SystemsGetAsync(long SystemId)
        {
            return await SystemsService.SystemsGetAsync(SystemId);
        }
        /// <summary>
        /// 子系统模型更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<CommonResult> SystemsUpdateAsync(SystemsUpdateDto SystemsUpdateDto, long SystemId)
        {
            return await SystemsService.SystemsUpdateAsync(SystemsUpdateDto, SystemId);
        }
        /// <summary>
        /// 子系统模型删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<CommonResult> SystemsDeleteAsync(List<long> SystemIds)
        {
            return await SystemsService.SystemsDeleteAsync(SystemIds);
        }
    }
}
