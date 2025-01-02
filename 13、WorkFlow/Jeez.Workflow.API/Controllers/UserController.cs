using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController
    {
        private IUserService UsersService { get; set; }
        public UserController(IUserService usersService)
        {
            UsersService = usersService;
        }
    }
}
