using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController
    {
        private IRoleService RoleService { get; set; }

        public RoleController(IRoleService roleService)
        {
            RoleService = roleService;
        }
    }
}
