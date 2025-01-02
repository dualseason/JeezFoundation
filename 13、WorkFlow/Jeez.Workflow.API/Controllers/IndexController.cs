using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IndexController
    {
        private IIndexService IndexService { get; set; }

        public IndexController(IIndexService indexService) 
        {
            IndexService = indexService;
        }
        
    }
}
