using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using JeezFoundation.Core.Domain.Entities;
using JeezFoundation.Core.Domain.Permission;

namespace Jeez.Workflow.API.Services.implements
{
    public class ResourceService : IResourceService
    {
        public Task<bool> AddAsync(ResourceShowDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BoxSaveAsync(RoleTreeDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(long[] ids, long userid)
        {
            throw new NotImplementedException();
        }

        public Task<List<ZTree>> GetBoxTreeAsync(long roleid)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResourceTree>> GetLeftTreeAsync(long userid)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceShowResult> GetResourceAsync(long id, long systemid)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceCategoryResult> GetTreeAsync(long systemId)
        {
            throw new NotImplementedException();
        }

        public Task<UserPermission> GetUserPermissionAsync(long userid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ResourceShowDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
