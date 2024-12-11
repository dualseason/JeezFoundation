namespace Jeez.Workflow.API.Contexts
{
    public class WorkflowFixtrue
    {
        public readonly IWorkflowDbContext Db;

        private readonly string ConnectionString = "Database=workflow;Data Source=localhost;User Id=root;Password=root;SslMode=none;charset=utf8mb4";

        public WorkflowFixtrue(IConfiguration configuration)
        {
            string MySQLAddress = configuration["MySQL:Connection"] ?? ConnectionString;
            Db = new WorkflowDbContext(MySQLAddress);
        }
    }
}
