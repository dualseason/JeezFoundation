using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface ISystemsService
    {
        /// <summary>
        /// 子系统模型创建
        /// </summary>
        /// <returns></returns>
        Task<CommonResult> SystemsCreateAsync(SystemsCreateDto systemsCreateDto);

        /// <summary>
        /// 子系统模型集合查询
        /// </summary>
        /// <param name="systemsGetListDto"></param>
        /// <returns></returns>
        Task<CommonResult<List<SystemsDto>>> SystemsGetListAsync(SystemsGetListDto systemsGetListDto);

        /// <summary>
        /// 子系统模型集合分页查询
        /// </summary>
        /// <param name="systemsGetListPageDto"></param>
        /// <returns></returns>
        Task<CommonPageResult<SystemsDto>> SystemsGetListPageAsync(SystemsGetListPageDto systemsGetListPageDto);

        /// <summary>
        /// 子系统模型查询【根据子系统模型Id查询】
        /// </summary>
        /// <param name="SystemId"></param>
        /// <returns></returns>
        Task<CommonResult<SystemsDto>> SystemsGetAsync(long SystemId);

        /// <summary>
        /// 子系统模型更新
        /// </summary>
        /// <param name="SystemsUpdateDto"></param>
        /// <param name="SystemId"></param>
        /// <returns></returns>
        Task<CommonResult> SystemsUpdateAsync(SystemsUpdateDto SystemsUpdateDto, long SystemId);

        /// <summary>
        /// 子系统模型删除
        /// </summary>
        /// <param name="SystemIds"></param>
        /// <returns></returns>
        Task<CommonResult> SystemsDeleteAsync(List<long> SystemIds);
    }
}
