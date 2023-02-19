using Rybalka.Core.Dto.User;

namespace Rybalka.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers(CancellationToken ct);
        Task CreateUser(UserDto userDto, CancellationToken ct);
        Task<bool> DeleteUser(int id, CancellationToken ct);
    }
}
