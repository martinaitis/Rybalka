using Microsoft.AspNetCore.Mvc;
using Rybalka.Core.Dto.Auth;
using Rybalka.Core.Interfaces.Services;
using RybalkaWebAPI.Attributes.Action;

namespace RybalkaWebAPI.Controllers
{
    [ServiceFilter(typeof(LogAttribute))]
    [Route("api/auth")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Register(
            [FromBody] RegisterDto registerDto,
            CancellationToken ct)
        {
            await _authService.Register(registerDto, ct);
            return StatusCode(StatusCodes.Status201Created);
        }

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken ct)
        {
            var login = await _authService.Login(loginDto, ct);
            if (!login.IsSuccess)
            {
                return StatusCode(login.StatusCode, login.ErrorMessage);
            }

            return Ok(login.Result);
        }
    }
}
