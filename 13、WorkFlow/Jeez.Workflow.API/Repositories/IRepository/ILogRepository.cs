using Jeez.Workflow.API.Model;
using JeezFoundation.Dapper;

namespace Jeez.Workflow.API.Repositories.IRepository
{
    public interface ILogRepository: IDapperRepository<SysLog>
    {
    }
}
