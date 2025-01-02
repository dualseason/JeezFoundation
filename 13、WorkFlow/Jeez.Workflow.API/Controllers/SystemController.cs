using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SystemController
    {
        private ISystemsService SystemsService { get; set; }

        public SystemController(ISystemsService systemsService) 
        {
            SystemsService = systemsService;
        }
    }
}
