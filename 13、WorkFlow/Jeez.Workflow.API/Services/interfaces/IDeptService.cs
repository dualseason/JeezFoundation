using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface IDeptService
    {
        /// <summary>
        /// 部门模型创建
        /// </summary>
        /// <param name="DeptCreateDto"></param>
        /// <returns></returns>
        Task<CommonResult> DeptCreateAsync(DeptCreateDto DeptCreateDto);

        /// <summary>
        /// 部门模型集合查询
        /// </summary>
        /// <param name="DeptGetListDto"></param>
        /// <returns></returns>
        Task<CommonResult<List<DeptDto>>> DeptGetListAsync(DeptGetListDto DeptGetListDto);

        /// <summary>
        /// 部门模型集合分页查询
        /// </summary>
        /// <param name="DeptGetListPageDto"></param>
        /// <returns></returns>
        Task<CommonPageResult<DeptDto>> DeptGetListPageAsync(DeptGetListPageDto DeptGetListPageDto);

        /// <summary>
        /// 部门模型查询【根据部门模型Id查询】
        /// </summary>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        Task<CommonResult<DeptSelectResultDto>> DeptGetAsync(long DeptId);

        /// <summary>
        /// 部门模型更新
        /// </summary>
        /// <param name="DeptUpdateDto"></param>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        Task<CommonResult> DeptUpdateAsync(DeptUpdateDto DeptUpdateDto, long DeptId);

        /// <summary>
        /// 部门模型删除
        /// </summary>
        /// <param name="DeptIds"></param>
        /// <returns></returns>
        Task<CommonResult> DeptDeleteAsync(List<long> DeptIds);
    }
}
