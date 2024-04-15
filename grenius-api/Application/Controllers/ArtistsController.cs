using AutoMapper;
using grenius_api.Application.Extensions;
using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Domain.Entities;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace grenius_api.Application.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistsController: ControllerBase
    {
        private readonly ILogger<ArtistsController> _logger;
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
    
        public ArtistsController(GreniusContext db, 
            ILogger<ArtistsController> logger, 
            IMapper mapper,
            IDistributedCache cache)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [SwaggerOperation(Summary ="Get a list of artists")]
        [SwaggerResponse(200, Type = typeof(List<ArtistResponseDTO>))]
        public async Task<IActionResult> GetArtists(CancellationToken cancellationToken, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            return Ok(_mapper.Map<List<ArtistResponseDTO>>(
                 await _db.Artists
                .Skip((pageIndex-1)*pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get artist by id")]
        [SwaggerResponse(200, Type = typeof(ArtistResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetArtistById([SwaggerParameter("Artist Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            string cacheKey = $"artist_{id}";
            var cachedArtist = await _cache.GetRecordAsync<ArtistResponseDTO>(cancellationToken, cacheKey);
            if (cachedArtist != null)
            {
                return Ok(cachedArtist);
            }
            else
            {
                Artist? _artist = await _db.Artists.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
                if (_artist is null)
                {
                    _logger.LogWarning("No artist with this id was found");
                    return NotFound();
                }
                else
                {
                    var responseArtist = _mapper.Map<ArtistResponseDTO>(_artist);
                    await _cache.SetRecordAsync(cancellationToken, cacheKey, responseArtist);
                    return Ok(responseArtist);
                }

            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add artist")]
        [SwaggerResponse(200, Type = typeof(ArtistResponseDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AddArtist([SwaggerRequestBody("Artist details")] ArtistRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }

            var entity = _db.Artists.Add(new Artist
            {
                Name = model.Name,
                Surname = model.Surname,
                Nickname = model.Nickname,
                Country = model.Country,
                Birthday = model.Birthday
            }).Entity;

            await _db.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(AddArtist), new { Id = entity.Id }, _mapper.Map<ArtistResponseDTO>(entity));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Artist")]
        [SwaggerResponse(200, Type = typeof(ArtistResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> UpdateArtist([SwaggerParameter("Artist Id")] int id, [SwaggerRequestBody("Artist details")] ArtistRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var entity = await _db.Artists.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No artist with this id was found");
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Surname = model.Surname;
            entity.Nickname = model.Surname;
            entity.Country = model.Country;
            entity.Birthday = model.Birthday;

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<ArtistResponseDTO>(entity));
            
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove artist")]
        [SwaggerResponse(200, Type = typeof(ArtistResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> DeleteArtist([SwaggerParameter("Artist Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("Id must be greater than 0"); 
                return BadRequest("Id must be greater than 0");
            }
            Artist? _artist = await _db.Artists.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (_artist is null)
            {
                _logger.LogWarning("No artist with this id was found");
                return NotFound();                
            }
            
            _db.Remove(_artist);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }

    }
}
