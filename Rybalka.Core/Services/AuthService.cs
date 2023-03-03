using Microsoft.IdentityModel.Tokens;
using Rybalka.Core.Dto;
using Rybalka.Core.Dto.Auth;
using Rybalka.Core.Entities;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Core.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rybalka.Core.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<ServiceResponseDto> Login(LoginDto login, CancellationToken ct)
        {
            var user = await _authRepository.GetUserByUsernameReadOnly(login.Username, ct);
            if (user == null)
            {
                return new ServiceResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found.",
                    StatusCode = 404
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                return new ServiceResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Wrong password",
                    StatusCode = 401
                };
            }

            return new ServiceResponseDto();
        }

        public string CreateToken(LoginDto login)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var symmetricSecurityKey = Environment.GetEnvironmentVariable("SYMMETRIC_SECURITY_KEY");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task Register(RegisterDto register, CancellationToken ct)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(register.Password);
            await _authRepository.CreateUser(
                new User 
                { 
                    Username = register.Username,
                    PasswordHash = passwordHash,
                    Password = passwordHash 
                },
                ct);
        }
    }
}
