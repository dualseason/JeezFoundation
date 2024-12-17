using Jeez.Workflow.API.Commons;
using Jeez.Workflow.API.Dtos;
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

        /// <summary>
        /// 首页查询
        /// </summary>
        [HttpGet]
        public async Task<CommonResult<IndexDto>> IndexGetAsync(IndexGetDto indexGetDto)
        {
            return await IndexService.IndexGetAsync(indexGetDto);
        }
    }
}
