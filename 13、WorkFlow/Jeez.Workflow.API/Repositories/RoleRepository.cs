using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class RoleRepository : DapperRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {

        }
    }
}
