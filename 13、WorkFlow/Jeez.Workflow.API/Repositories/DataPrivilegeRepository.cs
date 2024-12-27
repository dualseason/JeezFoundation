using Jeez.Workflow.API.Models.DataPrivilege;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class DataPrivilegeRepository : DapperRepository<DataPrivilege>, IDataPrivilegeRepository
    {
        public DataPrivilegeRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {

        }
    }
}
