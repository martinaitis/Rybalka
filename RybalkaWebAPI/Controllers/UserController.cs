using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rybalka.Core.Interfaces.Services;
using RybalkaWebAPI.Attributes.Action;

namespace RybalkaWebAPI.Controllers
{
    [ServiceFilter(typeof(LogAttribute))]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
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
