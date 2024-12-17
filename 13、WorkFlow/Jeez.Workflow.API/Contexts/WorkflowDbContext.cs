using Jeez.Workflow.API.Repositories;
using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper.DbContext;
using JeezFoundation.Dapper.SqlGenerator;
using MySqlConnector;

namespace Jeez.Workflow.API.Contexts
{
    public class WorkflowDbContext : DapperDbContext, IWorkflowDbContext
    {
        public WorkflowDbContext(string ConnectionString) : base(new MySqlConnection(ConnectionString))
        {

        }

        /// <summary>
        /// 数据库选择配置
        /// 使用MySQL
        /// </summary>
        private readonly SqlGeneratorConfig sqlGeneratorConfig = new SqlGeneratorConfig
        {
            SqlConnector = ESqlConnector.MySQL,
            // 表和列名使用引号
            UseQuotationMarks = true 
        };

        /// <summary>
        /// 实现子系统模型仓储
        /// </summary>
        public ISystemsRepository Systems => new SystemsRepository(Connection, sqlGeneratorConfig);

        public IUserRepository Users => new UserRepository(Connection, sqlGeneratorConfig);

        public IDeptRepository Dept => new DeptRepository(Connection, sqlGeneratorConfig);

        public IResourceRepository Resource => new ResourceRepository(Connection, sqlGeneratorConfig);

        public IRoleRepository Role => new RoleRepository(Connection, sqlGeneratorConfig);

        public IRoleResourceRepository RoleResource => new RoleResourceRepository(Connection, sqlGeneratorConfig);

        public IUserDeptRepository UserDept => new UserDeptRepository(Connection, sqlGeneratorConfig);

        public IUserRepository User => new UserRepository(Connection, sqlGeneratorConfig);

        public IUserRoleRepository UserRole => new UserRoleRepository(Connection, sqlGeneratorConfig);
    }
}
