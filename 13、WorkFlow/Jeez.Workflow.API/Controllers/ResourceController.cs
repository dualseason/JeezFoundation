using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    /// <summary>
    /// 资源【菜单】模型控制器
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ResourceController
    {
        private IResourceService ResourceService { get; set; }

        public ResourceController(IResourceService resourceService) 
        {
            ResourceService = resourceService;
        }
    }
}
