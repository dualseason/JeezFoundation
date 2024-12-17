using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class RoleResourceRepository : DapperRepository<RoleResource>, IRoleResourceRepository
    {
        public RoleResourceRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {

        }
    }
}
