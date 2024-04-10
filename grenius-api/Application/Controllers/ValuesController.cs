using grenius_api.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace grenius_api.Application.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("first")]
        public IActionResult Get(int id)
        {
            return NotFound("ahahah");
        }
        [HttpGet]
        [Route("first2/")]
        public IActionResult Get2(int id)
        {
            throw new Exception("Entity not found");
        }
        [HttpGet]
        [Route("first3/")]
        public IActionResult Get3(int id)
        {
            return NotFound();
        }
        [HttpGet]
        [Route("first4/")]
        public IActionResult Get4(int id)
        {
            return NotFound();
        }
    }
}
