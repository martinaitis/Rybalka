using Microsoft.AspNetCore.Mvc;
using Rybalka.Core.Dto;
using Rybalka.Core.Dto.Auth;
using Rybalka.Core.Interfaces.Services;
using RybalkaWebAPI.Attributes;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            var registerResponse = await _authService.Register(registerDto);
            if (registerResponse.IsSuccess)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return BadRequest(registerResponse.ErrorMessage);
        }

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (await _authService.Login(loginDto))
            {
                return Ok();
            }

            return Unauthorized($"{loginDto.UserName} unauthorized to login");
        }

        [Route("logout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            
            return Ok();
        }
    }
}
