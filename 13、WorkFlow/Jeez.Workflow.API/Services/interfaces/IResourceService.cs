using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface IResourceService
    {
        /// <summary>
        /// 资源【菜单】模型创建
        /// </summary>
        /// <param name="ResourceCreateDto"></param>
        /// <returns></returns>
        Task<CommonResult> ResourceCreateAsync(ResourceCreateDto ResourceCreateDto);

        /// <summary>
        /// 资源【菜单】模型集合查询
        /// </summary>
        /// <param name="ResourceGetListDto"></param>
        /// <returns></returns>
        Task<CommonResult<List<ResourceDto>>> ResourceGetListAsync([FromQuery] ResourceGetListDto ResourceGetListDto);

        /// <summary>
        /// 资源【菜单】模型集合分页查询
        /// </summary>
        /// <param name="ResourceGetListPageDto"></param>
        /// <returns></returns>
        Task<CommonPageResult<ResourceDto>> ResourceGetListPageAsync([FromQuery] ResourceGetListPageDto ResourceGetListPageDto);

        /// <summary>
        /// 资源【菜单】模型查询【根据资源【菜单】模型Id查询】
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        Task<CommonResult<ResourceSelectResultDto>> ResourceGetAsync(long ResourceId);

        /// <summary>
        /// 资源【菜单】模型更新
        /// </summary>
        /// <param name="ResourceUpdateDto"></param>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        Task<CommonResult> ResourceUpdateAsync(ResourceUpdateDto ResourceUpdateDto, long ResourceId);

        /// <summary>
        /// 资源【菜单】模型删除
        /// </summary>
        /// <param name="ResourceIds"></param>
        /// <returns></returns>
        Task<CommonResult> ResourceDeleteAsync(List<long> ResourceIds);
    }
}
