using Rybalka.Core.Entities;

namespace Rybalka.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersReadOnly();
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByIdReadOnly(int id);
        Task CreateUser(User user);
        Task DeleteUser(User user);
    }
}
