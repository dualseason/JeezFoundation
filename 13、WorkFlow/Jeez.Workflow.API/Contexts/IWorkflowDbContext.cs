using Jeez.Workflow.API.Repositories.IRepository;

namespace Jeez.Workflow.API.Contexts
{
    public interface IWorkflowDbContext
    {
        public ISystemsRepository Systems { get; }
    }
}
