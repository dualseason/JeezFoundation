using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class SystemsRepository : DapperRepository<Systems>, ISystemsRepository
    {
        public SystemsRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {

        }

        public async Task<IEnumerable<Systems>> SystemsGetListPageAsync(SystemsGetListPageDto SystemsGetListPage)
        {
            string sqlWhere = $@" WHERE 1 = 1 AND IsDel = {SystemsGetListPage.IsDel} ";
            string sql = $"SELECT * from sys_system {sqlWhere} limit {SystemsGetListPage.OffSet()}, {SystemsGetListPage.PageSize}";
            return await QueryAsync(sql);
        }

        public async Task<int> SystemsGetListCountPageAsync(SystemsGetListPageDto SystemsGetListPage)
        {
            string sqlWhere = $@" WHERE 1 = 1 AND IsDel = {SystemsGetListPage.IsDel} ";
            return await ExecuteScalarAsync<int>($"SELECT COUNT(1) FROM sys_system {sqlWhere} ");
        }
    }
}
