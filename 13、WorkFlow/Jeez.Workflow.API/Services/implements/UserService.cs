using AutoMapper;
using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Services.interfaces;

namespace Jeez.Workflow.API.Services.implements
{
    public class UserService : IUserService
    {
        public WorkflowFixtrue WorkflowFixtrue { get; set; }

        public IMapper Mapper { get; set; }

        public UserService(WorkflowFixtrue workflowFixtrue, IMapper mapper)
        {
            WorkflowFixtrue = workflowFixtrue;
            Mapper = mapper;
        }
    }
}
