using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Models;
using JeezFoundation.Dapper;

namespace Jeez.Workflow.API.Repositories.IRepository
{
    /// <summary>
    /// 子系统模型仓储接口
    /// </summary>
    public interface ISystemsRepository : IDapperRepository<Systems>
    {
        Task<IEnumerable<Systems>> SystemsGetListPageAsync(SystemsGetListPageDto SystemsGetListPage);

        Task<int> SystemsGetListCountPageAsync(SystemsGetListPageDto SystemsGetListPage);
    }
}
