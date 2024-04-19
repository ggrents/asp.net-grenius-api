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
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;

        public RolesController(GreniusContext db, ILogger<RolesController> logger,
           IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of roles")]
        [SwaggerResponse(200, Type = typeof(List<RoleResponseDTO>))]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<RoleResponseDTO>>(await _db.Roles.ToListAsync(cancellationToken)));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get role by id")]
        [SwaggerResponse(200, Type = typeof(RoleResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetRoleById([SwaggerParameter("Role Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            Role? _role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (_role is null)
            {
                _logger.LogWarning("No role with this id was found");
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<RoleResponseDTO>(_role));
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        [SwaggerOperation(Summary = "Add role")]
        [SwaggerResponse(200, Type = typeof(RoleResponseDTO))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AddRole([SwaggerRequestBody("Role details")] RoleRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }

            var entity = _db.Roles.Add(new Role
            {
                Name = model.Name,
            }).Entity;

            await _db.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(AddRole), new { Id = entity.Id }, _mapper.Map<RoleResponseDTO>(entity));
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update role")]
        [SwaggerResponse(200, Type = typeof(RoleResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> UpdateRole([SwaggerParameter("Role Id")] int id, [SwaggerRequestBody("Role details")] RoleRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var entity = await _db.Roles.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No role with this id was found");
                return NotFound();
            }

            entity.Name = model.Name;
            
            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<RoleResponseDTO>(entity));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove role")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> DeleteRole([SwaggerParameter("Role Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("Id must be greater than 0");
                return BadRequest("Id must be greater than 0");
            }
            Role? _role = await _db.Roles.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (_role is null)
            {
                _logger.LogWarning("No role with this id was found");
                return NotFound();
            }

            _db.Remove(_role);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
