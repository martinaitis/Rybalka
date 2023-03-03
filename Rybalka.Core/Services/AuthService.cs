using Microsoft.AspNetCore.Identity;
using Rybalka.Core.Dto;
using Rybalka.Core.Dto.Auth;
using Rybalka.Core.Interfaces.Services;

namespace Rybalka.Core.Services
{
    public sealed class AuthService : IAuthService
    {
        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
        }

/*        public async Task<bool> Login(LoginDto login)
        {
        }

        public async Task<ServiceResponseDto> Register(RegisterDto register)
        {
        }*/
    }
}
