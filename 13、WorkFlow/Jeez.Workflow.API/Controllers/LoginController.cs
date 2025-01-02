using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
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
    }
}
