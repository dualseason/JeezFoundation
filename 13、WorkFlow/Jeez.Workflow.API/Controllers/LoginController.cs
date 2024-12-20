using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JeezFoundation.Core.Extensions;
using JeezFoundation.Core.Domain.Entities;


namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController: ControllerBase
    {
        private ILoginService LoginService { get; set; }

        private UserIdentity UserIdentity => base.User.ToUserIdentity();

        public LoginController(ILoginService loginService)
        {
            LoginService = loginService;
        }

        /// <summary>
        /// 用户登录接口
        /// </summary>
        [HttpPost]
        public async Task<CommonResult<UserIdentity>> UserLoginAsync(UserLoginDto userLoginDto)
        {
            CommonResult<UserIdentity> result = await LoginService.UserLoginAsync(userLoginDto);

            if (result.Success && result.Data != null) 
            {
                UserIdentity userIdentity = result.Data;
                var list = userIdentity.ToClaims();

                ClaimsIdentity identity = new(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaims(list);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            }
            return result;
        }

        /// <summary>
        /// 用户登录结果获取
        /// </summary>
        [HttpPost("SysUser")]
        [Authorize]
        public CommonResult<UserIdentity> GetUserLoginResult()
        {
            return new CommonResult<UserIdentity>(true, "查询成功", UserIdentity);
        }

        [HttpPost("Logout")]
        public async Task<CommonResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new CommonResult(true, "注销成功");
        }
    }
}
