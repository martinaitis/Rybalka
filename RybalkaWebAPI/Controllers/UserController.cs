using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rybalka.Core.Dto.User;
using Rybalka.Core.Interfaces.Services;
using RybalkaWebAPI.Attributes;

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
        public async Task<ActionResult<IEnumerable<string>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users.Any())
            {
                return Ok(users);
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest($"Request body does not contains {nameof(UserDto)}");
            }

            await _userService.CreateUser(userDto);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.DeleteUser(id))
            {
                return NoContent();
            }

            return NotFound($"User with id:{id} does not exist in DB");
        }

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (userDto == null 
                || userDto.UserName.IsNullOrEmpty() 
                || userDto.Password.IsNullOrEmpty())
            {
                return BadRequest("Empty login data");
            }

            if (await _userService.Login(userDto))
            {
                return Ok();
            }

            return Unauthorized($"{userDto.UserName} unauthorized to login");
        }
    }
}
