using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController
    {
        private IUserService UsersService { get; set; }

        public UserController(IUserService usersService)
        {
            UsersService = usersService;
        }

        /// <summary>
        /// 用户创建
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResult> UserCreateAsync(UserCreateDto userCreateDto)
        {
            return await UsersService.UserCreateAsync(userCreateDto);
        }

        /// <summary>
        /// 用户集合查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResult<List<UserDto>>> UserGetListAsync([FromQuery] UserGetListDto userGetListDto)
        {
            return await UsersService.UserGetListAsync(userGetListDto);
        }


        /// <summary>
        /// 用户集合分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<CommonPageResult<UserDto>> UserGetListPageAsync(UserGetListPageDto userGetListPageDto)
        {
            return await UsersService.UserGetListPageAsync(userGetListPageDto);
        }

        /// <summary>
        /// 用户查询【根据用户Id查询】
        /// </summary>
        /// <returns></returns>
        [HttpGet("{UserId}")]
        public async Task<CommonResult<UserDto>> UserGetAsync([FromQuery] long UserId)
        {
            return await UsersService.UserGetAsync(UserId);
        }

        /// <summary>
        /// 用户更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<CommonResult> UserUpdateAsync(UserUpdateDto userUpdateDto, long UserId)
        {
            return await UsersService.UserUpdateAsync(userUpdateDto, UserId);
        }

        /// <summary>
        /// 用户删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<CommonResult> UserDeleteAsync(List<long> UserIds)
        {
            return await UsersService.UserDeleteAsync(UserIds);
        }

        /// <summary>
        /// 用户部门查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserDept")]
        public async Task<CommonResult<UserDeptDto>> UserDeptGetAsync(long UserId)
        {
            return await UsersService.UserDeptGetAsync(UserId);
        }

        /// <summary>
        /// 用户部门分配
        /// </summary>
        /// <returns></returns>
        [HttpPut("UserDept")]
        public async Task<CommonResult> UserDeptAssignAsync(UserDeptAssignDto userDeptAssignDto)
        {
            return await UsersService.UserDeptAssignAsync(userDeptAssignDto);
        }

        /// <summary>
        /// 用户角色查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserRole")]
        public async Task<CommonResult<UserRoleDto>> UserRoleGetAsync(long UserId)
        {
            return await UsersService.UserRoleGetAsync(UserId);
        }

        /// <summary>
        /// 用户角色分配
        /// </summary>
        /// <returns></returns>
        [HttpPut("UserRole")]
        public async Task<CommonResult> UserRoleAssignAsync(UserRoleAssignDto userRoleAssignDto)
        {
            return await UsersService.UserRoleAssignAsync(userRoleAssignDto);
        }
    }
}
