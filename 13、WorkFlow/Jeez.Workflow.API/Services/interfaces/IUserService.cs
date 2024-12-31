using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// 用户创建
        /// </summary>
        /// <param name="userCreateDto"></param>
        /// <returns></returns>
        Task<CommonResult> UserCreateAsync(UserCreateDto userCreateDto);
        /// <summary>
        /// 用户集合查询
        /// </summary>
        /// <param name="userGetListDto"></param>
        /// <returns></returns>
        Task<CommonResult<List<UserDto>>> UserGetListAsync(UserGetListDto userGetListDto);
        /// <summary>
        /// 用户集合分页查询
        /// </summary>
        /// <param name="userGetListPageDto"></param>
        /// <returns></returns>
        Task<CommonPageResult<UserDto>> UserGetListPageAsync(UserGetListPageDto userGetListPageDto);
        /// <summary>
        /// 用户查询【根据用户Id查询】
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<CommonResult<UserDto>> UserGetAsync(long UserId);
        /// <summary>
        /// 用户更新
        /// </summary>
        /// <param name="userUpdateDto"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<CommonResult> UserUpdateAsync(UserUpdateDto userUpdateDto, long UserId);
        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="UserIds"></param>
        /// <returns></returns>
        Task<CommonResult> UserDeleteAsync(List<long> UserIds);
        /// <summary>
        /// 用户部门查询
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<CommonResult<UserDeptDto>> UserDeptGetAsync(long UserId);
        /// <summary>
        /// 用户部门分配
        /// </summary>
        /// <param name="userDeptAssignDto"></param>
        /// <returns></returns>
        Task<CommonResult> UserDeptAssignAsync(UserDeptAssignDto userDeptAssignDto);
        /// <summary>
        /// 用户角色查询
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<CommonResult<UserRoleDto>> UserRoleGetAsync(long UserId);
        /// <summary>
        /// 用户角色分配
        /// </summary>
        /// <param name="userRoleAssignDto"></param>
        /// <returns></returns>
        Task<CommonResult> UserRoleAssignAsync(UserRoleAssignDto userRoleAssignDto);
    }
}
