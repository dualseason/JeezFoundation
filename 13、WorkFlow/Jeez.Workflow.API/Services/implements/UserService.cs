using AutoMapper;
using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Contexts;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Models;
using Jeez.Workflow.API.Services.interfaces;
using JeezFoundation.Core.Extensions;

namespace Jeez.Workflow.API.Services.implements
{
    public class UserService : IUserService
    {
        public WorkflowFixtrue WorkflowFixtrue { get; set; }

        public IMapper Mapper { get; set; }

        public UserService(WorkflowFixtrue workflowFixtrue, IMapper mapper)
        {
            WorkflowFixtrue = workflowFixtrue;
            Mapper = mapper;
        }

        public async Task<CommonResult> UserCreateAsync(UserCreateDto userCreateDto)
        {
            using (var tran = WorkflowFixtrue.Db.BeginTransaction())
            {
                try
                {
                    User user = Mapper.Map<User>(userCreateDto);
                    await WorkflowFixtrue.Db.User.InsertAsync(user, tran);

                    UserDept userDept = new UserDept(user.UserId, userCreateDto.DeptId);
                    await WorkflowFixtrue.Db.UserDept.InsertAsync(userDept, tran);

                    tran.Commit();
                    return new CommonResult(true, "新增成功");
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return new CommonResult(false, ex.Message);
                }
            }
        }

        public async Task<CommonResult<List<UserDto>>> UserGetListAsync(UserGetListDto userGetListDto)
        {
            IEnumerable<User> users = await WorkflowFixtrue.Db.User.FindAllAsync(u => u.IsDel == userGetListDto.IsDel);
            List<UserDto> list = Mapper.Map<List<UserDto>>(users);
            return new CommonResult<List<UserDto>>(true, "查询成功", list);
        }

        public async Task<CommonPageResult<UserDto>> UserGetListPageAsync(UserGetListPageDto userGetListPageDto)
        {
            // 查询所有未删除的用户
            IEnumerable<User> users = await WorkflowFixtrue.Db.User.FindAllAsync(u => u.IsDel == userGetListPageDto.IsDel);
            // 将用户列表映射为UserDto列表
            List<UserDto> list = Mapper.Map<List<UserDto>>(users);

            // 计算实际的取值范围，防止超出列表范围
            int skipCount = userGetListPageDto.OffSet();
            int takeCount = userGetListPageDto.PageSize;
            int actualTakeCount = takeCount > (list.Count - skipCount) ? list.Count - skipCount : takeCount;

            // 实现分页
            var pagedList = list.Skip(skipCount).Take(actualTakeCount).ToList();

            return new CommonPageResult<UserDto>(userGetListPageDto.PageSize, list.Count, pagedList);
        }

        public async Task<CommonResult<UserDto>> UserGetAsync(long UserId)
        {
            User user = await WorkflowFixtrue.Db.User.FindAsync(m => m.UserId == UserId && m.IsDel == false);
            UserDto userDto = Mapper.Map<UserDto>(user);

            UserDept userDept = await WorkflowFixtrue.Db.UserDept.FindAsync(m => m.UserId == user.UserId);
            userDto.DepartId = userDept.DeptId;
            return new CommonResult<UserDto>(true, "查询成功", userDto);
        }

        public Task<CommonResult> UserUpdateAsync(UserUpdateDto userUpdateDto, long UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonResult> UserDeleteAsync(List<long> UserIds)
        {
            using (var transaction = WorkflowFixtrue.Db.BeginTransaction())
            {
                var users = await WorkflowFixtrue.Db.User.FindAllAsync(m => m.IsDel == false && UserIds.Contains(m.UserId));
                foreach (var user in users)
                {
                    user.IsDel = true;
                    await WorkflowFixtrue.Db.User.UpdateAsync(user, transaction);
                }
                transaction.Commit();
                return new CommonResult(true, "删除成功");
            }
        }

        public Task<CommonResult<UserDeptDto>> UserDeptGetAsync(long UserId)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult> UserDeptAssignAsync(UserDeptAssignDto userDeptAssignDto)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult<UserRoleDto>> UserRoleGetAsync(long UserId)
        {
            throw new NotImplementedException();
        }

        public Task<CommonResult> UserRoleAssignAsync(UserRoleAssignDto userRoleAssignDto)
        {
            throw new NotImplementedException();
        }
    }
}
