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
    [Authorize(Roles = "admin")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly GreniusContext _db;
        private readonly IMapper _mapper;
        IUserService _userService;
        public UsersController(GreniusContext db, ILogger<UsersController> logger,
          IMapper mapper, IUserService userService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }

        private async Task<bool> UserExists(string username, CancellationToken cancellationToken)
        {
            return await _db.Users.AnyAsync(x => x.Username == username, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of users")]
        [SwaggerResponse(200, Type = typeof(List<UserResponseDTO>))]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<UserResponseDTO>>(await _db.Users.ToListAsync(cancellationToken)));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user by id")]
        [SwaggerResponse(200, Type = typeof(UserResponseDTO))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetUserById([SwaggerParameter("User Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("The entered id is less than 1");
                return BadRequest("Id must be greater than 0");
            }

            User? _user = await _db.Users.FirstOrDefaultAsync(u =>u.Id == id, cancellationToken);
            if (_user is null)
            {
                _logger.LogWarning("No user with this id was found");
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<UserResponseDTO>(_user));
            }
        }


        [HttpGet("role/{id}")]
        [SwaggerOperation(Summary = "Get a list of users by role")]
        [SwaggerResponse(200, Type = typeof(List<UserResponseDTO>))]
        public async Task<IActionResult> GetUsersByRole([SwaggerParameter("Role Id")] int id, CancellationToken cancellationToken)
        {
            return Ok(_mapper.Map<List<UserResponseDTO>>(await _db.Users.Where(u=>u.RoleId==id).ToListAsync(cancellationToken)));
        }

        [HttpPatch("{id}/change-role")]
        [SwaggerOperation(Summary = "Change the role of a user")]
        [SwaggerResponse(200, Type = typeof(UserResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> ChangeRole([SwaggerParameter("User Id")] int id, [SwaggerRequestBody("Role Id")] int roleId, CancellationToken cancellationToken)
        {
            var entity = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No user with this id was found");
                return NotFound();
            }

            entity.RoleId = roleId;
            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<UserResponseDTO>(entity));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update user")]
        [SwaggerResponse(200, Type = typeof(UserResponseDTO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> UpdateUser([SwaggerParameter("User Id")] int id, [SwaggerRequestBody("User details")] UserRequestDTO model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request body : @{model}", model);
                return BadRequest("Invalid request body");
            }
            var entity = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("No user with this id was found");
                return NotFound();
            }

            entity.Username = model.Username;
            entity.Email = model.Email;
            entity.IsActive = model.IsActive;
            entity.RoleId = model.RoleId;

            await _db.SaveChangesAsync(cancellationToken);
            return Ok(_mapper.Map<UserResponseDTO>(entity));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove user")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        public async Task<IActionResult> DeleteUser([SwaggerParameter("User Id")] int id, CancellationToken cancellationToken)
        {
            if (id < 1)
            {
                _logger.LogWarning("Id must be greater than 0");
                return BadRequest("Id must be greater than 0");
            }
            User? _user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (_user is null)
            {
                _logger.LogWarning("No user with this id was found");
                return NotFound();
            }

            _db.Remove(_user);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Registration of a user")]
        [SwaggerResponse(201)]
        [SwaggerResponse(400)]
        public async Task<IActionResult> Register(UserRegisterRequestDTO _user, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Enter username and password");
            }

            if (await UserExists(_user.Username, cancellationToken))
            {
                return BadRequest("Username is already taken");
            }
            if (!_userService.IsPasswordValid(_user.Password))
            {
                return BadRequest("Password is not valid");
            }

            var user = new User
            {
                Email = _user.Email,
                Username = _user.Username,
                DateCreated = DateTime.UtcNow,
                IsActive = true,
                PasswordHash = _userService.HashPassword(_user.Password),
                RoleId = 4
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync(cancellationToken);
            return Created();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Authentication of a user")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        public async Task<ActionResult<UserLoginResponseDTO>> Login(UserLoginRequestDTO _user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Enter username and password");
            }

            var user = await _db.Users
                .Include(u=>u.Role)
                .SingleOrDefaultAsync(x => x.Username == _user.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            if (!_userService.ValidatePassword(_user.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid password");
            }

            return new UserLoginResponseDTO
            {
                UserId = user.Id,
                Token = _userService.CreateToken(user)
            };

        }

        //[HttpPost]
        //[SwaggerOperation(Summary = "Refresh the access token")]


    }
}
