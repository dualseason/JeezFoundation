using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;

namespace Jeez.Workflow.API.Services.implements
{
    public class DeptService : IDeptService
    {
        public Task<CommonResult> DeptCreateAsync(DeptCreateDto DeptCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult> DeptDeleteAsync(List<long> DeptIds)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult<DeptSelectResultDto>> DeptGetAsync(long DeptId)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult<List<DeptDto>>> DeptGetListAsync(DeptGetListDto DeptGetListDto)
        {
            throw new NotImplementedException();
        }

        public Task<CommonPageResult<DeptDto>> DeptGetListPageAsync(DeptGetListPageDto DeptGetListPageDto)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult> DeptUpdateAsync(DeptUpdateDto DeptUpdateDto, long DeptId)
        {
            throw new NotImplementedException();
        }
    }
}
