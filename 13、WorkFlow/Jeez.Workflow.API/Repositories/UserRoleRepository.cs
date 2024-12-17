using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class UserRoleRepository : DapperRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {

        }
    }
}
