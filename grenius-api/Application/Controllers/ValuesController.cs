using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace grenius_api.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;    
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public List<int> Get()
        {
            _logger.LogCritical("123");
            return [1, 2, 3];
        }
    }
}
