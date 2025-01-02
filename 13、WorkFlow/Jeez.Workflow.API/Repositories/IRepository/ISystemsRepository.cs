using Jeez.Workflow.API.Model;
using JeezFoundation.Dapper;

namespace Jeez.Workflow.API.Repositories.IRepository
{
    /// <summary>
    /// 子系统模型仓储接口
    /// </summary>
    public interface ISystemsRepository : IDapperRepository<SysSystem>
    {
    }
}
