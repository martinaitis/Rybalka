using Rybalka.Core.Entities;

namespace Rybalka.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task CreateUser(User user, CancellationToken ct);
        Task<User?> GetUserByUsernameReadOnly(string username, CancellationToken ct);
    }
}
