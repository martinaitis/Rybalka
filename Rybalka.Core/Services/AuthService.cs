using Microsoft.AspNetCore.Identity;
using Rybalka.Core.Dto;
using Rybalka.Core.Dto.Auth;
using Rybalka.Core.Interfaces.Services;

namespace Rybalka.Core.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager= userManager;
            _signInManager= signInManager;
        }

        public async Task<bool> Login(LoginDto login)
        {
            return (await _signInManager.PasswordSignInAsync(
                login.UserName,
                login.Password,
                login.RememberMe,
                false))
                .Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync(); 
        }

        public async Task<ServiceResponseDto> Register(RegisterDto register)
        {
            var newUser = new IdentityUser()
            {
                UserName = register.UserName
            };

            var registerResult = await _userManager.CreateAsync(newUser, register.Password);
            var response = new ServiceResponseDto
            {
                IsSuccess = registerResult.Succeeded
            };

            if (response.IsSuccess)
            {
                return response;
            }

            response.ErrorMessage = registerResult.Errors.First().Description;

            return response;
        }
    }
}
