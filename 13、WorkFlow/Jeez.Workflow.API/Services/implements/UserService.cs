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

        public async Task<CommonResult<UserDto>> UserCreateAsync(UserCreateDto userCreateDto)
        {
            User user = Mapper.Map<User>(userCreateDto);
            UserDto userDto = Mapper.Map<UserDto>(user);
            if (await WorkflowFixtrue.Db.User.InsertAsync(user))
            {
                return new CommonResult<UserDto>(true, "创建成功", userDto);
            }
            else
            {
                return new CommonResult<UserDto>(false, "创建失败", new UserDto());
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
            List<User> userList = await WorkflowFixtrue.Db.User.UserGetListPageAsync(userGetListPageDto);
            List<UserDto> data = Mapper.Map<List<UserDto>>(userList);
            int total = await WorkflowFixtrue.Db.User.UserGetListCountPageAsync(userGetListPageDto);
            return new CommonPageResult<UserDto>(userGetListPageDto.PageSize, total, data);
        }

        public async Task<CommonResult<UserDto>> UserGetAsync(long UserId)
        {
            User user = await WorkflowFixtrue.Db.User.FindAsync(m => m.UserId == UserId);
            UserDto userDto = Mapper.Map<UserDto>(user);
            if (user.IsNotNull())
            {
                return new CommonResult<UserDto>(true, "查询成功", userDto);
            }
            else
            {
                return new CommonResult<UserDto>(false,"查询失败", new UserDto());
            }
        }

        public async Task<CommonResult> UserUpdateAsync(UserUpdateDto userUpdateDto, long UserId)
        {
            User user = await WorkflowFixtrue.Db.User.FindAsync(m => m.UserId == UserId);
            if (user == null)
            {
                throw new Exception("User不存在");
            }
            user = Mapper.Map<User>(userUpdateDto);
            var b = await WorkflowFixtrue.Db.User.UpdateAsync(user);
            if (b)
            {
                return new CommonResult(true, "更新成功");
            }
            else
            {
                return new CommonResult(false, "更新失败");
            }
        }

        public async Task<CommonResult> UserDeleteAsync(List<long> UserIds)
        {
            using (var transaction = WorkflowFixtrue.Db.BeginTransaction())
            {
                var users = await WorkflowFixtrue.Db.User.FindAllAsync(m => m.IsDel == false && UserIds.Contains(m.UserId));
                foreach (var user in users)
                {
                    // 逻辑删除
                    user.IsDel = true;
                    await WorkflowFixtrue.Db.User.UpdateAsync(user, transaction);
                }
                transaction.Commit();
                return new CommonResult(true, "删除成功");
            }
        }

        public async Task<CommonResult<UserDeptDto>> UserDeptGetAsync(long UserId)
        {
            var user = await WorkflowFixtrue.Db.User.FindByIdAsync(UserId);

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
