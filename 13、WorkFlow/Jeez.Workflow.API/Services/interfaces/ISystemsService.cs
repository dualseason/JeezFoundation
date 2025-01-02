using Jeez.Workflow.API.Model;
using JeezFoundation.Core.Domain.Entities;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface ISystemsService
    {
        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SysSystem> GetByIdAsync(long id);

        /// <summary>
        /// 新增系统
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(SysSystem system);

        /// <summary>
        /// 更新系统
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(SysSystem system);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(List<long> ids, long userid);

        /// <summary>
        /// 异步获取系统数据
        /// </summary>
        /// <returns></returns>
        Task<List<SysSystem>> ListAsync();

        /// <summary>
        /// 页面返回系统数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Page<SysSystem>> GetPageAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 禁用系统
        /// </summary>
        /// <returns></returns>
        Task<bool> DisableSystemAsync(long ids);

        /// <summary>
        /// 启用系统
        /// </summary>
        /// <param name="id">系统ID</param>
        /// <returns></returns>
        Task<bool> EnableSystemAsync(long id);
    }
}
