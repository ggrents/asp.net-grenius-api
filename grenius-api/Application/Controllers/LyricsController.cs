using AutoMapper;
using grenius_api.Application.Extensions;
using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Domain.Entities;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace grenius_api.Application.Controllers
{
    [Authorize]
    [Route("api/lyrics")]
    [ApiController]
    public class LyricsController : ControllerBase
    {
        private readonly ILogger<LyricsController> _logger;
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public LyricsController(GreniusContext db, ILogger<LyricsController> logger,
            IMapper mapper, IDistributedCache cache)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of lyrics")]
        [SwaggerResponse(200, Type = typeof(List<LyricsResponseDTO>))]
        public async Task<IActionResult> GetLyricsList(CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<LyricsResponseDTO>>(await _db.Lyrics.ToListAsync(cancellationToken)));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get lyrics by id")]
        [SwaggerResponse(200, Type = typeof(LyricsResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetLyricsById([SwaggerParameter("Lyrics Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            string cacheKey = $"lyrics_{id}";
            var cachedLyrics = await _cache.GetRecordAsync<LyricsResponseDTO>(cancellationToken, cacheKey);
            if (cachedLyrics != null)
            {
                return Ok(cachedLyrics);
            }
            else
            { 
                Lyrics? _lyrics = await _db.Lyrics.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
                if (_lyrics is null)
                {
                    _logger.LogWarning("No lyrics with this id was found");
                    return NotFound();
                }
                else
                {
                    var responseLyrics = _mapper.Map<LyricsResponseDTO>(_lyrics);
                    await _cache.SetRecordAsync(cancellationToken, cacheKey, responseLyrics);
                    return Ok(responseLyrics);
                }
            }
        }

        [HttpGet("song/{id}")]
        [SwaggerOperation(Summary = "Get a list of lyrics by song Id")]
        [SwaggerResponse(200, Type = typeof(List<LyricsResponseDTO>))]
        public async Task<IActionResult> GetLyricsBySong([SwaggerParameter("Song Id")] int id, CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<LyricsResponseDTO>>(await _db.Lyrics.Where(l=>l.SongId==id).ToListAsync(cancellationToken)));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add lyrics")]
        [SwaggerResponse(200, Type = typeof(LyricsResponseDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AddLyrics([SwaggerRequestBody("Lyrics details")] LyricsRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }

            int.TryParse(User.Identity!.Name, out int parsedUserId);
            
                var entity = _db.Lyrics.Add(new Lyrics
            {
                SongId = model.SongId,
                Text = model.Text,
                UserCreatedId = parsedUserId
                }).Entity;

            await _db.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(AddLyrics), new { Id = entity.Id }, _mapper.Map<LyricsResponseDTO>(entity));
        }

        [Authorize(Roles = "editor,admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update lyrics")]
        [SwaggerResponse(200, Type = typeof(LyricsResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> UpdateLyrics([SwaggerParameter("Lyrics Id")] int id, [SwaggerRequestBody("Lyrics details")] LyricsRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var entity = await _db.Lyrics.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No lyrics with this id was found");
                return NotFound();
            }

            int.TryParse(User.Identity!.Name, out int userId);
            if (entity.UserCreatedId != userId)
            {
                return Forbid("YoYou cannot edit text that you have not created");
            }

            entity.SongId = model.SongId;
            entity.Text = model.Text;

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<LyricsResponseDTO>(entity));

        }

        [Authorize(Roles = "editor,admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove lyrics")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> DeleteLyrics([SwaggerParameter("Lyrics Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("Id must be greater than 0");
                return BadRequest("Id must be greater than 0");
            }

            Lyrics? _lyrics = await _db.Lyrics.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (_lyrics is null)
            {
                _logger.LogWarning("No lyrics with this id was found");
                return NotFound();
            }

            _db.Remove(_lyrics);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
