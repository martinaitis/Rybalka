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

            _logger.LogWarning($"Action: {nameof(GetUsers)} Message: {nameof(users)} table is empty");
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogWarning($"Action: {nameof(PostUser)} Message: {nameof(userDto)} == null");
                return BadRequest(userDto);
            }

            User user = _mapper.Map<User>(userDto);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
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
                _logger.LogWarning($"Action: {nameof(Login)} Message: Empty login data");
                return BadRequest();
            }

            var isAuthorized = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            if (isAuthorized != null)
            {
                return Ok();
            }

            _logger.LogWarning($"Action: {nameof(Login)} Message: {username} unauthorized to login");
            return Unauthorized();
        }
    }
}
