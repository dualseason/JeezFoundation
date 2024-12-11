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
        /// </summary>
        private readonly SqlGeneratorConfig sqlGeneratorConfig = new SqlGeneratorConfig
        {
            SqlConnector = ESqlConnector.MySQL, // 使用MySQL
            UseQuotationMarks = true // 表和列名使用引号
        };

        /// <summary>
        /// 实现子系统模型仓储
        /// </summary>
        public ISystemsRepository Systems => new SystemsRepository(Connection, sqlGeneratorConfig);
    }
}
