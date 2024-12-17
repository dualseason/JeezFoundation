using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;

namespace Jeez.Workflow.API.Services.interfaces
{
    public interface ILoginService
    {
        Task<CommonResult<UserLoginResultDto>> UserLoginAsync(UserLoginDto userLoginDto);
    }
}
