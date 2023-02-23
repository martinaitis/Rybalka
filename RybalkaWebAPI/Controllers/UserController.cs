using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rybalka.Core.Dto.User;
using Rybalka.Core.Interfaces.Services;
using RybalkaWebAPI.Attributes.Action;

namespace RybalkaWebAPI.Controllers
{
    [ServiceFilter(typeof(LogAttribute))]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(
            IMapper mapper,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <remarks>
        /// Return all users list.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<string>>> GetUsers(CancellationToken ct)
        {
            var users = await _userService.GetAllUsers(ct);
            if (users.Any())
            {
                return Ok(users);
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostUser([FromBody] UserDto userDto, CancellationToken ct)
        {
            if (userDto == null)
            {
                return BadRequest($"Request body does not contains {nameof(UserDto)}");
            }

            await _userService.CreateUser(userDto, ct);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
        {
            if (await _userService.DeleteUser(id, ct))
            {
                return NoContent();
            }

            return NotFound($"User with id:{id} does not exist in DB");
        }
    }
}
