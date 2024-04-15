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
    [Route("api/songs")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ILogger<SongsController> _logger;
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public SongsController(GreniusContext db, ILogger<SongsController> logger,
            IMapper mapper, IDistributedCache cache)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of songs")]
        [SwaggerResponse(200, Type = typeof(List<SongResponseDTO>))]
        public async Task<IActionResult> GetSongs(CancellationToken cancellationToken, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            return Ok(_mapper.Map<List<SongResponseDTO>>(await _db.Songs.Include(s=>s.Features)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)));
        }

        [HttpGet("album/{id}")]
        [SwaggerOperation(Summary = "Get a list of songs by album")]
        [SwaggerResponse(200, Type = typeof(List<SongResponseDTO>))]
        public async Task<IActionResult> GetSongsByAlbum([SwaggerParameter("Album Id")]  int id , CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<SongResponseDTO>>(await _db.Songs.Include(s => s.Features).Where(s=>s.AlbumId==id).ToListAsync(cancellationToken)));
        }

        [HttpGet("genre/{id}")]
        [SwaggerOperation(Summary = "Get a list of songs by genre")]
        [SwaggerResponse(200, Type = typeof(List<SongResponseDTO>))]
        public async Task<IActionResult> GetSongsByGenre([SwaggerParameter("Genre Id")] int id, CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<SongResponseDTO>>(await _db.Songs.Include(s => s.Features).Where(s => s.GenreId == id).ToListAsync(cancellationToken)));
        }

        [HttpGet("producer/{id}")]
        [SwaggerOperation(Summary = "Get a list of songs by producer")]
        [SwaggerResponse(200, Type = typeof(List<SongResponseDTO>))]
        public async Task<IActionResult> GetSongsByProducer([SwaggerParameter("Producer Id")] int id, CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<SongResponseDTO>>(await _db.Songs.Include(s => s.Features).Where(s => s.ProducerId == id).ToListAsync(cancellationToken)));
        }

        [HttpGet("artist/{id}")]
        [SwaggerOperation(Summary = "Get a list of songs by artist")]
        [SwaggerResponse(200, Type = typeof(List<SongResponseDTO>))]
        public async Task<IActionResult> GetSongsByArtist([SwaggerParameter("Artist Id")] int id, CancellationToken cancellationToken)
        {
            var songList = await _db.Songs
            .Include(s => s.Features)
            .Where(s => s.ArtistId == id || s.Features.Any(f => f.ArtistId == id))
            .ToListAsync(cancellationToken);

            return Ok(_mapper.Map<List<SongResponseDTO>>(songList));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get song by id")]
        [SwaggerResponse(200, Type = typeof(SongResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetSongById([SwaggerParameter("Song Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            string cacheKey = $"song_{id}";
            var cachedSong = await _cache.GetRecordAsync<SongResponseDTO>(cancellationToken, cacheKey);
            if (cachedSong != null)
            {
                return Ok(cachedSong);
            }
            else
            {
                Song? _song = await _db.Songs.Include(s => s.Features).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
                if (_song is null)
                {
                    _logger.LogWarning("No artist with this id was found");
                    return NotFound();
                }
                else
                {
                    var responseSong = _mapper.Map<SongResponseDTO>(_song);
                    await _cache.SetRecordAsync(cancellationToken, cacheKey, responseSong);
                    return Ok(responseSong);
                }
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add song")]
        [SwaggerResponse(200, Type = typeof(SongResponseDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AddSong([SwaggerRequestBody("Song details")] SongRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body: {@model}", model);
                return BadRequest("Invalid request body");
            }

            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                   
                    var songEntity = _db.Songs.Add(new Song
                    {
                        Title = model.Title,
                        ReleaseDate = model.ReleaseDate,
                        IsFeature = model.IsFeature,
                        ArtistId = model.ArtistId,
                        AlbumId = model.AlbumId,
                        ProducerId = model.ProducerId,
                        GenreId = model.GenreId
                    }).Entity;

                    await _db.SaveChangesAsync(cancellationToken);

                    if (model.IsFeature && model.Features != null && model.Features.Any())
                    {
                        var features = model.Features.Select(f => new Feature
                        {
                            Priority = f.Priority,
                            ArtistId = f.ArtistId,
                            SongId = songEntity.Id
                        });

                        _db.Features.AddRange(features);
                        await _db.SaveChangesAsync(cancellationToken);
                    }

                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(AddSong), new {Id = songEntity.Id}, _mapper.Map<SongResponseDTO>(songEntity));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "An error occurred while processing your request.");
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update song")]
        [SwaggerResponse(200, Type = typeof(SongResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> UpdateSong([SwaggerParameter("Song Id")] int id, [SwaggerRequestBody("Song details")] SongUpdateRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var entity = await _db.Songs.Include(s=>s.Features).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No song with this id was found");
                return NotFound();
            }

            entity.Title = model.Title;
            entity.ReleaseDate = model.ReleaseDate;
            entity.IsFeature = model.IsFeature;
            entity.AlbumId = model.AlbumId;
            entity.GenreId = model.GenreId;
            entity.ArtistId = model.ArtistId;
            entity.ProducerId = model.ProducerId;

            if (model.Features != null && model.Features.Any())
            {
                var requestFeatureIds = model.Features.Where(f => f.Id.HasValue).Select(f => f.Id.Value).ToList();

                foreach (var featureRequest in model.Features)
                {
                    if (featureRequest.Id.HasValue)
                    {
                        var existingFeature = entity.Features.FirstOrDefault(f => f.Id == featureRequest.Id);
                        if (existingFeature != null)
                        {
                            existingFeature.Priority = featureRequest.Priority;
                            existingFeature.ArtistId = featureRequest.ArtistId;
                        }
                    }
                    else
                    {
                        var newFeatures = model.Features.Where(f => !f.Id.HasValue).Select(f => new Feature
                        {
                            Priority = f.Priority,
                            ArtistId = f.ArtistId,
                            SongId = entity.Id
                        }).ToList();
                        _db.Features.AddRange(newFeatures);
                    }
                }

                var featuresToRemove = entity.Features.Where(f => !requestFeatureIds.Contains(f.Id)).ToList();
                _db.Features.RemoveRange(featuresToRemove);
            }
            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<SongResponseDTO>(entity));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove song")]
        [SwaggerResponse(200, Type = typeof(SongResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> DeleteSong([SwaggerParameter("Song Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("Id must be greater than 0");
                return BadRequest("Id must be greater than 0");
            }
            Song? _song= await _db.Songs.Include(s=>s.Features).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (_song is null)
            {
                _logger.LogWarning("No song with this id was found");
                return NotFound();
            }

            _db.RemoveRange(_song.Features);
            _db.Remove(_song);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }

    }
}
