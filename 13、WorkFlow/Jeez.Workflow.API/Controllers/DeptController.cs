using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeptController
    {
        private IDeptService DeptService { get; set; }

        public DeptController(IDeptService deptService)
        {
            DeptService = deptService;
        }

        /// <summary>
        /// 部门模型创建
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeptCreateAsync")]
        public async Task<CommonResult> DeptCreateAsync(DeptCreateDto DeptCreateDto)
        {
            return await DeptService.DeptCreateAsync(DeptCreateDto);
        }

        /// <summary>
        /// 部门模型集合查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeptGetListAsync")]
        public async Task<CommonResult<List<DeptDto>>> DeptGetListAsync(DeptGetListDto DeptGetListDto)
        {
            return await DeptService.DeptGetListAsync(DeptGetListDto);
        }

        /// <summary>
        /// 部门模型集合分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("DeptGetListPageAsync")]
        public async Task<CommonPageResult<DeptDto>> DeptGetListPageAsync(DeptGetListPageDto DeptGetListPageDto)
        {
            return await DeptService.DeptGetListPageAsync(DeptGetListPageDto);
        }

        /// <summary>
        /// 部门模型查询【根据部门模型Id查询】
        /// </summary>
        /// <returns></returns>
        [HttpGet("{DeptId}")]
        public async Task<CommonResult<DeptSelectResultDto>> DeptGetAsync(long DeptId)
        {
            return await DeptService.DeptGetAsync(DeptId);
        }
        /// <summary>
        /// 部门模型更新
        /// </summary>
        /// <returns></returns>
        [HttpPut("DeptUpdateAsync")]
        public async Task<CommonResult> DeptUpdateAsync(DeptUpdateDto DeptUpdateDto, long DeptId)
        {
            return await DeptService.DeptUpdateAsync(DeptUpdateDto, DeptId);
        }
        /// <summary>
        /// 部门模型删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("DeptDeleteAsync")]
        public async Task<CommonResult> DeptDeleteAsync(List<long> DeptIds)
        {
            return await DeptService.DeptDeleteAsync(DeptIds);
        }
    }
}
