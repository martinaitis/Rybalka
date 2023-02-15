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
        public async Task<List<User>> GetUsersReadOnly()
        {
            return await _db.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(n => n.Id == id);
        }
        public async Task<User?> GetUserByIdReadOnly(int id)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task CreateUser(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
    }
}
