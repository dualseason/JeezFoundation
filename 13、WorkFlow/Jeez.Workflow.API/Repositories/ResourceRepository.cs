using Jeez.Workflow.API.Model;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper;
using JeezFoundation.Dapper.SqlGenerator;
using System.Data;

namespace Jeez.Workflow.API.Repositories
{
    public class ResourceRepository : DapperRepository<SysResource>, IResourceRepository
    {
        public ResourceRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config)
        {

        }

        /// <summary>
        /// 根据用户ID获取该用户可用的菜单
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public async Task<IEnumerable<SysResource>> GetListByUserIdAsync(long userid)
        {

            string sql = $@"SELECT res.* FROM sys_resource res
LEFT JOIN sys_role_resource srr ON srr.ResourceId=res.ResourceId
LEFT JOIN sys_role sr ON sr.RoleId = srr.RoleId
LEFT JOIN sys_user_role sur ON sur.RoleId=sr.RoleId
LEFT JOIN sys_user su ON su.UserId=sur.UserId
WHERE res.IsDel=0 AND res.IsButton=0 AND res.IsShow=1 AND sr.IsDel=0 AND su.IsDel=0 AND su.UserId={userid}  ORDER BY sort ASC";

            return await this.QueryAsync(sql);
        }
    }
}
