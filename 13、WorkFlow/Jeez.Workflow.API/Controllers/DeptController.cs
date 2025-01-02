using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Workflow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeptController
    {
        private IDeptService DeptService { get; set; }

        public DeptController(IDeptService deptService)
        {
            DeptService = deptService;
        }
        
    }
}
