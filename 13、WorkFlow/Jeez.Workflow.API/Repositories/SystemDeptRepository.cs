using Jeez.Workflow.API.Models.SystemDept;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class SystemDeptRepository : DapperRepository<SystemDept>, ISystemDeptRepository
    {
        public SystemDeptRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {
        }
    }
}
