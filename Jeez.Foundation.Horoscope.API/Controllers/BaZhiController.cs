using Jeez.Foundation.Horoscope.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jeez.Foundation.Horoscope.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaZhiController: ControllerBase
    {
        private readonly ILogger<BaZhiController> _logger;
        private readonly IBaZhiService _bazhiService;
        public BaZhiController(IBaZhiService baZhi ,ILogger<BaZhiController> logger)
        {
            _bazhiService = baZhi;
            _logger = logger;
        }

        [HttpPost("ganzhi-year")]
        public String GetGanZhiYear(DateTime date)
        {
            return _bazhiService.GetGanZhiYear(date.ToString("yyyy-MM-dd"));
        }
    }
}
