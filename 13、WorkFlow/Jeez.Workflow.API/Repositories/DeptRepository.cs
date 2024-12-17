using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class DeptRepository : DapperRepository<Dept>, IDeptRepository
    {
        public DeptRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {

        }
    }
}
