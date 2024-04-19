using AutoMapper;
using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Domain.Entities;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace grenius_api.Application.Controllers
{
    [Authorize]
    [Route("api/producers")]
    [ApiController]
    public class ProducersController: ControllerBase
    {
        private readonly ILogger<ProducersController> _logger;
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;
    
        public ProducersController(GreniusContext db, 
            ILogger<ProducersController> logger, 
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary ="Get a list of producers")]
        [SwaggerResponse(200, Type = typeof(List<ProducerResponseDTO>))]
        public async Task<IActionResult> GetProducers(CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<ProducerResponseDTO>>(
                 await _db.Producers
                .ToListAsync(cancellationToken)));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get producer by id")]
        [SwaggerResponse(200, Type = typeof(ProducerResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetProducerById([SwaggerParameter("Producer Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            Producer? __producer = await _db.Producers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (__producer is null)
            {
                _logger.LogWarning("No producer with this id was found");
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<ProducerResponseDTO>(__producer));
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add producer")]
        [SwaggerResponse(200, Type = typeof(ProducerResponseDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AddProducer([SwaggerRequestBody("Producer details")] ProducerRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }

            var entity = _db.Producers.Add(new Producer
            {
                Name = model.Name,
                Surname = model.Surname,
                Nickname = model.Nickname,
                Country = model.Country
            }).Entity;

            await _db.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(AddProducer), new { Id = entity.Id }, _mapper.Map<ProducerResponseDTO>(entity));
        }

        [Authorize(Roles = "editor,admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update producer")]
        [SwaggerResponse(200, Type = typeof(ProducerResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> UpdateProducer([SwaggerParameter("Producer Id")] int id, [SwaggerRequestBody("Producer details")] ProducerRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var entity = await _db.Producers.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No producer with this id was found");
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Surname = model.Surname;
            entity.Nickname = model.Surname;
            entity.Country = model.Country;

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<ProducerResponseDTO>(entity));           
        }

        [Authorize(Roles = "editor,admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove producer")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> DeleteProducer([SwaggerParameter("Producer Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("Id must be greater than 0"); 
                return BadRequest("Id must be greater than 0");
            }

            Producer? _producer = await _db.Producers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (_producer is null)
            {
                _logger.LogWarning("No producer with this id was found");
                return NotFound();                
            }
            
            _db.Remove(_producer);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
