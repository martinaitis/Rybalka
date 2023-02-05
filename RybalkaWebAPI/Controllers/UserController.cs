using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RybalkaWebAPI.Data;
using RybalkaWebAPI.Models.Dto.User;
using RybalkaWebAPI.Models.Entity;

namespace RybalkaWebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UserController(
            ILogger<UserController> logger,
            ApplicationDbContext db,
            IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        /// <remarks>
        /// Return all users list.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<string>>> GetUsers()
        {
            var users = await _db.Users.Select(u => u.Username).ToListAsync();

            if (users.Any())
            {
                return Ok(users);
            }
            var message = $"{nameof(UserDto)} table is empty";
            _logger.LogWarning(message);
            return NotFound(message);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                var message = $"Request body does not contains {nameof(UserDto)}";
                _logger.LogWarning(message);
                return BadRequest(message);
            }

            User user = _mapper.Map<User>(userDto);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _db.Users.FirstOrDefault(n => n.Id == id);
            if (user == null)
            {
                var message = $"User with id:{id} does not exist in DB";
                _logger.LogWarning($"Action: {nameof(DeleteUser)} Message: {message}");
                return NotFound(message);
            }
            else
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return NoContent();
            }
        }

        [Route("login")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                _logger.LogWarning("Empty login data");
                return BadRequest("Empty login data");
            }

            var isAuthorized = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            if (isAuthorized != null)
            {
                return Ok();
            }
            var message = $"{username} unauthorized to login";
            _logger.LogWarning(message);
            return Unauthorized(message);
        }
    }
}
