using grenius_api.Application.Repositories;
using grenius_api.Domain.Exceptions;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace grenius_api.Application.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistsController: ControllerBase
    {
        private readonly ILogger<ArtistsController>? _logger;
        private readonly IArtistsRepository _artistsRepository;

        public ArtistsController(IArtistsRepository repository)
        {
            _artistsRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _artistsRepository.GetArtists());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtistById(int id)
        {
            var artist = await _artistsRepository.GetArtist(id);
            if (artist is null)
            {
                return NotFound();
            }
            return Ok(artist);
        }
    }
}
