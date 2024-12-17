using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;

namespace Jeez.Workflow.API.Services.interfaces
{
    /// <summary>
    /// 首页Service接口
    /// </summary>
    public interface IIndexService
    {
        public Task<CommonResult<IndexDto>> IndexGetAsync(IndexGetDto indexGetDto);
    }
}
