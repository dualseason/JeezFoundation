using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos.Systems;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface ISystemsService
    {
        /// <summary>
        /// 子系统模型创建
        /// </summary>
        /// <returns></returns>
        Task<CommonResult<SystemsDto>> SystemsCreateAsync(SystemsCreateDto systemsCreateDto);

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
    }
}
