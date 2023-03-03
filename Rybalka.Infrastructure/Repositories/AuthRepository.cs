using Microsoft.EntityFrameworkCore;
using Rybalka.Core.Entities;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Infrastructure.Data;

namespace Rybalka.Infrastructure.Repositories
{
    public sealed class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        public AuthRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateUser(User user, CancellationToken ct)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<User?> GetUserByUsernameReadOnly(string username, CancellationToken ct)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(n => n.Username == username, ct);
        }
    }
}
