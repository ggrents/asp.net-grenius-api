using AutoMapper;
using grenius_api.Application.Models.Requests;
using grenius_api.Application.Models.Responses;
using grenius_api.Application.Services;
using grenius_api.Domain.Entities;
using grenius_api.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Cryptography;
using System.Text;

namespace grenius_api.Application.Controllers
{
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
                PasswordHash = _userService.HashPassword(_user.Password)
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync(cancellationToken);
            return Created();
        }


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
                .Include(u=>u.UserRoles)
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


    }
}
