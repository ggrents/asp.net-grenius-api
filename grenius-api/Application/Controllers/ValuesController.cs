using grenius_api.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace grenius_api.Application.Controllers
{
    [Route("api/musicians")]
    [ApiController]
    public class MusicianController: ControllerBase
    {
        private readonly ILogger<MusicianController> _logger;

        [HttpGet]
        public int Get()
        {
            throw new Exception();
        }
    }
}
