using AutoMapper;
using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Application.Services;
using grenius_api.Domain.Entities;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace grenius_api.Application.Controllers
{
    [Authorize]
    [Route("api/annotations")]
    [ApiController]
    public class AnnotationsController : ControllerBase
    {
        private readonly ILogger<AnnotationsController> _logger;
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;
        private readonly IAnnotationService _service;
        public AnnotationsController(GreniusContext db, ILogger<AnnotationsController> logger,
            IMapper mapper, IAnnotationService service) { 
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of annotations")]
        [SwaggerResponse(200, Type = typeof(List<AnnotationResponseDTO>))]
        public async Task<IActionResult> GetAnnotationsList(CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<AnnotationResponseDTO>>(await _db.Annotations.Include(a=>a.Lyrics).ToListAsync(cancellationToken)));
        }


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get annotation by id")]
        [SwaggerResponse(200, Type = typeof(AnnotationResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetAnnotationById([SwaggerParameter("Annotation Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            Annotation? _annotation = await _db.Annotations.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (_annotation is null)
            {
                _logger.LogWarning("No genre with this id was found");
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<AnnotationResponseDTO>(_annotation));
            }
        }

        [HttpGet("lyrics/{id}")]
        [SwaggerOperation(Summary = "Get annotations by lyrics id")]
        [SwaggerResponse(200, Type = typeof(AnnotationResponseDTO))]
        public async Task<IActionResult> GetAnnotationByLyrics([SwaggerParameter("Lyrics Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            return Ok(_mapper.Map<List<AnnotationResponseDTO>>(await _db.Annotations.Where(a=>a.LyricsId==id).ToListAsync(cancellationToken)));
        }

        [HttpGet("song/{id}")]
        [SwaggerOperation(Summary = "Get annotations by song id")]
        [SwaggerResponse(200, Type = typeof(List<AnnotationResponseDTO>))]
        public async Task<IActionResult> GetAnnotationBySong([SwaggerParameter("Song Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            var annotations = await _db.Annotations
                .Where(a => a.Lyrics.SongId == id)
                .ToListAsync(cancellationToken);

            var annotationResponseDTOs = _mapper.Map<List<AnnotationResponseDTO>>(annotations);
            return Ok(annotationResponseDTOs);
        }

        
        [HttpPost]
        [SwaggerOperation(Summary = "Add Annotation")]
        [SwaggerResponse(200, Type = typeof(AnnotationResponseDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AddAnnotation([SwaggerRequestBody("Annotation details")] AnnotationRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var _lyrics = await _db.Lyrics.FirstOrDefaultAsync(l => l.Id == model.LyricsId, cancellationToken);
            
            if (_lyrics == null)
            {
                _logger.LogWarning("Lyrics with id {Id} not found", model.LyricsId);
                return BadRequest("Lyrics not found");
            }

            var annotation = _service.ConvertRequestToEntity(model, _lyrics);
            var entity = _db.Annotations.Add(annotation).Entity;
            await _db.SaveChangesAsync(cancellationToken);

            var responseDto = _mapper.Map<AnnotationResponseDTO>(entity);
            return CreatedAtAction(nameof(AddAnnotation), new { Id = entity.Id }, responseDto);
        }

        [Authorize(Roles = "editor,admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update annotation")]
        [SwaggerResponse(200, Type = typeof(AnnotationResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> UpdateAnnotation([SwaggerParameter("Annotation Id")] int id, [SwaggerRequestBody("Annotation details")] AnnotationUpdateRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var entity = await _db.Annotations.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No annotation with this id was found");
                return NotFound();
            }

            entity.StartSymbol = model.StartSymbol;
            entity.EndSymbol = model.EndSymbol;
            entity.Text = model.Text;
            entity.LyricsId = model.LyricsId;

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<AnnotationResponseDTO>(entity));

        }

        [Authorize(Roles = "editor,admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove annotation")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> DeleteAnnotation([SwaggerParameter("Annotation Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("Id must be greater than 0");
                return BadRequest("Id must be greater than 0");
            }
            Annotation? _annotation = await _db.Annotations.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (_annotation is null)
            {
                _logger.LogWarning("No annotation with this id was found");
                return NotFound();
            }

            _db.Remove(_annotation);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }


    }
}
