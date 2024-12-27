using Jeez.Workflow.API.Repositories.IRepository;
using JeezFoundation.Dapper.DbContext;

namespace Jeez.Workflow.API.Contexts
{
    public interface IWorkflowDbContext : IDapperDbContext
    {
        public IDeptRepository Dept { get; }

        public IResourceRepository Resource { get; }

        public IRoleRepository Role { get; }

        public IRoleResourceRepository RoleResource { get; }

        public ISystemsRepository Systems { get; }

        public IUserDeptRepository UserDept { get; }

        public IUserRepository User { get; }

        public IUserRoleRepository UserRole { get; }

        public IDataPrivilegeRepository DataPrivilege { get; }
    }
}