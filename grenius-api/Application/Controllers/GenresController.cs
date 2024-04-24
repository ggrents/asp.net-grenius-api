using AutoMapper;
using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Domain.Entities;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;


namespace grenius_api.Application.Controllers
{
    [Authorize]
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> _logger;
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;

        public GenresController(GreniusContext db, ILogger<GenresController> logger,
           IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of genres")]
        [SwaggerResponse(200, Type = typeof(List<GenreResponseDTO>))]
        public async Task<IActionResult> GetGenres([FromQuery] string message, [FromQuery] string key, CancellationToken cancellationToken)
        {
                return Ok(_mapper.Map<List<GenreResponseDTO>>(await _db.Genres.ToListAsync(cancellationToken)));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get genre by id")]
        [SwaggerResponse(200, Type = typeof(GenreResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetGenreById([SwaggerParameter("Genre Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            Genre? _genre = await _db.Genres.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (_genre is null)
            {
                _logger.LogWarning("No genre with this id was found");
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<GenreResponseDTO>(_genre));
            }
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Add genre")]
        [SwaggerResponse(200, Type = typeof(GenreResponseDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AddGenre([SwaggerRequestBody("Genre details")] GenreRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }

            var entity = _db.Genres.Add(new Genre
            {
                Name = model.Name,
                Description = model.Description
            }).Entity;

            await _db.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(AddGenre), new { Id = entity.Id }, _mapper.Map<GenreResponseDTO>(entity));
        }

        [Authorize(Roles = "editor,admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Genre")]
        [SwaggerResponse(200, Type = typeof(GenreResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> UpdateGenre([SwaggerParameter("Genre Id")] int id, [SwaggerRequestBody("Genre details")] GenreRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var entity = await _db.Genres.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No genre with this id was found");
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Description = model.Description;

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<GenreResponseDTO>(entity));

        }

        [Authorize(Roles = "editor,admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove genre")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> DeleteGenre([SwaggerParameter("Genre Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("Id must be greater than 0");
                return BadRequest("Id must be greater than 0");
            }
            Genre? _genre = await _db.Genres.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (_genre is null)
            {
                _logger.LogWarning("No genre with this id was found");
                return NotFound();
            }

            _db.Remove(_genre);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
