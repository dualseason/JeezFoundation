using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using JeezFoundation.Core.Domain.Entities;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface ILoginService
    {
        Task<CommonResult<UserIdentity>> UserLoginAsync(UserLoginDto userLoginDto);
    }
}
