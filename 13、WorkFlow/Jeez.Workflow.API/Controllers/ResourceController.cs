using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    /// <summary>
    /// 资源【菜单】模型控制器
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ResourceController
    {
        private IResourceService ResourceService { get; set; }

        public ResourceController(IResourceService resourceService) 
        {
            ResourceService = resourceService;
        }

        /// <summary>
        /// 资源【菜单】模型创建
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResult> ResourceCreateAsync(ResourceCreateDto ResourceCreateDto)
        {
            return await ResourceService.ResourceCreateAsync(ResourceCreateDto);
        }

        /// <summary>
        /// 资源【菜单】模型集合查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResult<List<ResourceDto>>> ResourceGetListAsync([FromQuery] ResourceGetListDto ResourceGetListDto)
        {
            return await ResourceService.ResourceGetListAsync(ResourceGetListDto);
        }

        /// <summary>
        /// 资源【菜单】模型集合分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<CommonPageResult<ResourceDto>> ResourceGetListPageAsync([FromQuery] ResourceGetListPageDto ResourceGetListPageDto)
        {
            return await ResourceService.ResourceGetListPageAsync(ResourceGetListPageDto);
        }
        /// <summary>
        /// 资源【菜单】模型查询【根据资源【菜单】模型Id查询】
        /// </summary>
        /// <returns></returns>
        [HttpGet("{ResourceId}")]
        public async Task<CommonResult<ResourceSelectResultDto>> ResourceGetAsync(long ResourceId)
        {
            return await ResourceService.ResourceGetAsync(ResourceId);
        }
        /// <summary>
        /// 资源【菜单】模型更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<CommonResult> ResourceUpdateAsync(ResourceUpdateDto ResourceUpdateDto, long ResourceId)
        {
            return await ResourceService.ResourceUpdateAsync(ResourceUpdateDto, ResourceId);
        }
        /// <summary>
        /// 资源【菜单】模型删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<CommonResult> ResourceDeleteAsync(List<long> ResourceIds)
        {
            return await ResourceService.ResourceDeleteAsync(ResourceIds);
        }
    }
}
