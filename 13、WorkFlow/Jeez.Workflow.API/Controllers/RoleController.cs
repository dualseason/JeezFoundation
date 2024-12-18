using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController
    {
        private IRoleService RoleService { get; set; }

        public RoleController(IRoleService roleService)
        {
            RoleService = roleService;
        }

        /// <summary>
        /// 角色模型创建
        /// </summary>
        /// <returns></returns>
        [HttpPost("RoleCreateAsync")]
        public async Task<bool> RoleCreateAsync(RoleCreateDto RoleCreateDto)
        {
            return await RoleService.RoleCreateAsync(RoleCreateDto);
        }

        /// <summary>
        /// 角色模型集合查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("RoleGetListAsync")]
        public async Task<List<RoleDto>> RoleGetListAsync(RoleGetListDto RoleGetListDto)
        {
            return await RoleService.RoleGetListAsync(RoleGetListDto);
        }

        /// <summary>
        /// 角色模型集合分页查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("RoleGetListPageAsync")]
        public async Task<RolePageDto> RoleGetListPageAsync(RoleGetListPageDto RoleGetListPageDto)
        {
            return await RoleService.RoleGetListPageAsync(RoleGetListPageDto);
        }
        /// <summary>
        /// 角色模型查询【根据角色模型Id查询】
        /// </summary>
        /// <returns></returns>
        [HttpGet("{RoleId}")]
        public async Task<RoleDto> RoleGetAsync(long RoleId)
        {
            return await RoleService.RoleGetAsync(RoleId);
        }
        /// <summary>
        /// 角色模型更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> RoleUpdateAsync(RoleUpdateDto RoleUpdateDto, long RoleId)
        {
            return await RoleService.RoleUpdateAsync(RoleUpdateDto, RoleId);
        }
        /// <summary>
        /// 角色模型删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> RoleDeleteAsync(List<long> RoleIds)
        {
            return await RoleService.RoleDeleteAsync(RoleIds);
        }
    }
}
