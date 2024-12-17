using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;


namespace Jeez.Workflow.API.Controllers
{
    public class LoginController: ControllerBase
    {
        private ILoginService LoginService { get; set; }

        private SystemUser SystemUser => base.User.ToSystemUser();

        public LoginController(ILoginService loginService)
        {
            LoginService = loginService;
        }

        /// <summary>
        /// 用户登录接口
        /// </summary>
        [HttpPost]
        public async Task<CommonResult<UserLoginResultDto>> UserLoginAsync(UserLoginDto userLoginDto)
        {
            // 1、登录结果获取
            CommonResult<UserLoginResultDto> result = await LoginService.UserLoginAsync(userLoginDto);
            if (result.Success && result.Data != null) 
            {
                UserLoginResultDto dto = result.Data;
                List<Claim> list = new List<Claim>();
                list.Add(new Claim(ClaimTypes.Name, dto.UserName ?? ""));
                list.Add(new Claim(ClaimTypes.Gender, dto.Sex.ToString()));
                list.Add(new Claim(ClaimTypes.Sid, dto.UserId.ToString()));
                list.Add(new Claim(ClaimTypes.Uri, dto.HeadImg ?? ""));
                list.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(dto.Other)));

                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
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
        public CommonResult<SystemUser> GetUserLoginResult()
        {
            return new CommonResult<SystemUser>(true, "查询成功", SystemUser);
        }

        [HttpPost("Logout")]
        public async Task<CommonResult> Logout()
        {
            //1、用户注销
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return new CommonResult(true, "注销成功");
        }
    }
}
