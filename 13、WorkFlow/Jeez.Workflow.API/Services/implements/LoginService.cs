using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;

namespace Jeez.Workflow.API.Services.implements
{
    public class LoginService : ILoginService
    {
        public Task<CommonResult<UserLoginResultDto>> UserLoginAsync(UserLoginDto userLoginDto)
        {
            throw new NotImplementedException();
        }
    }
}
