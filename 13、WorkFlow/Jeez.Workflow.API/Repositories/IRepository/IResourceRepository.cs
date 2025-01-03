using Jeez.Workflow.API.Model;
using JeezFoundation.Dapper;

namespace Jeez.Workflow.API.Repositories.IRepository
{
    public interface IResourceRepository : IDapperRepository<SysResource>
    {
        Task<IEnumerable<SysResource>> GetListByUserIdAsync(long userid);
    }
}
