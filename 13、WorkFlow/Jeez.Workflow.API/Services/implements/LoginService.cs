using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using JeezFoundation.Core.Domain.Entities;

namespace Jeez.Workflow.API.Services.implements
{
    public class LoginService : ILoginService
    {
        public Task<CommonResult<UserIdentity>> UserLoginAsync(UserLoginDto userLoginDto)
        {
            throw new NotImplementedException();
        }
    }
}
