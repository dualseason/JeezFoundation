using Jeez.Workflow.API.Dtos;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface IRoleService
    {
        /// <summary>
        /// 角色模型创建
        /// </summary>
        /// <param name="RoleCreateDto"></param>
        /// <returns></returns>
        Task<bool> RoleCreateAsync(RoleCreateDto RoleCreateDto);
        /// <summary>
        /// 角色模型集合查询
        /// </summary>
        /// <param name="RoleGetListDto"></param>
        /// <returns></returns>
        Task<List<RoleDto>> RoleGetListAsync(RoleGetListDto RoleGetListDto);
        /// <summary>
        /// 角色模型集合分页查询
        /// </summary>
        /// <param name="RoleGetListPageDto"></param>
        /// <returns></returns>
        Task<RolePageDto> RoleGetListPageAsync(RoleGetListPageDto RoleGetListPageDto);
        /// <summary>
        /// 角色模型查询【根据角色模型Id查询】
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        Task<RoleDto> RoleGetAsync(long RoleId);

        /// <summary>
        /// 角色模型更新
        /// </summary>
        /// <param name="RoleUpdateDto"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        Task<bool> RoleUpdateAsync(RoleUpdateDto RoleUpdateDto, long RoleId);
        /// <summary>
        /// 角色模型删除
        /// </summary>
        /// <param name="RoleIds"></param>
        /// <returns></returns>
        Task<bool> RoleDeleteAsync(List<long> RoleIds);
    }
}
