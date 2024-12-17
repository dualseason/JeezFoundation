using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Services.implements
{
    public class ResourceService : IResourceService
    {
        public Task<CommonResult> ResourceCreateAsync(ResourceCreateDto ResourceCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult> ResourceDeleteAsync(List<long> ResourceIds)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult<ResourceSelectResultDto>> ResourceGetAsync(long ResourceId)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult<List<ResourceDto>>> ResourceGetListAsync([FromQuery] ResourceGetListDto ResourceGetListDto)
        {
            throw new NotImplementedException();
        }

        public Task<CommonPageResult<ResourceDto>> ResourceGetListPageAsync([FromQuery] ResourceGetListPageDto ResourceGetListPageDto)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult> ResourceUpdateAsync(ResourceUpdateDto ResourceUpdateDto, long ResourceId)
        {
            throw new NotImplementedException();
        }
    }
}
