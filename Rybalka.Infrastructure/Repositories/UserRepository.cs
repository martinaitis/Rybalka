using Microsoft.EntityFrameworkCore;
using Rybalka.Core.Entities;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Infrastructure.Data;

namespace Rybalka.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<User>> GetUsersReadOnly(CancellationToken ct)
        {
            return await _db.Users.AsNoTracking().ToListAsync(ct);
        }

        public async Task<User?> GetUserById(int id, CancellationToken ct)
        {
            return await _db.Users.FirstOrDefaultAsync(n => n.Id == id, ct);
        }
        public async Task<User?> GetUserByIdReadOnly(int id, CancellationToken ct)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        public async Task CreateUser(User user, CancellationToken ct)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteUser(User user, CancellationToken ct)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync(ct);
        }
    }
}
