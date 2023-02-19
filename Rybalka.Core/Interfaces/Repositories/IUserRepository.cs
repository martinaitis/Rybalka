using Rybalka.Core.Entities;

namespace Rybalka.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersReadOnly(CancellationToken ct);
        Task<User?> GetUserById(int id, CancellationToken ct);
        Task<User?> GetUserByIdReadOnly(int id, CancellationToken ct);
        Task CreateUser(User user, CancellationToken ct);
        Task DeleteUser(User user, CancellationToken ct);
    }
}
