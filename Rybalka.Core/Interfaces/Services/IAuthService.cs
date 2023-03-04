using Rybalka.Core.Dto;
using Rybalka.Core.Dto.Auth;

namespace Rybalka.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResponseDto> Login(LoginDto login, CancellationToken ct);
        Task Register(RegisterDto register, CancellationToken ct);
    }
}
