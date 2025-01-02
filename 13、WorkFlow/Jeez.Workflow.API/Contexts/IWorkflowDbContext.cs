using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper.DbContext;

namespace Jeez.Workflow.API.Contexts
{
    public interface IWorkflowDbContext : IDapperDbContext
    {
        public IButtonRepository Button { get; }

        public IDataPrivilegeRepository DataPrivilege { get; }

        public IDeptLeaderRepository DeptLeader { get; }

        public IDeptRepository Dept { get; }

        public ILeaderRepository Leader { get; }

        public ILogRepository Log { get; }

        public IReleaseLogRepository ReleaseLog { get; }

        public IResourceRepository Resource { get; }

        public IRoleRepository Role { get; }

        public IRoleResourceRepository RoleResource { get; }

        public IScheduleRepository Schedule { get; }

        public ISystemsRepository Systems { get; }

        public IUserDeptRepository UserDept { get; }

        public IUserRepository User { get; }

        public IUserRoleRepository UserRole { get; }
    }
}