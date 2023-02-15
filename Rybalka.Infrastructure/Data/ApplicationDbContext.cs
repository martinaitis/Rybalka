using Microsoft.EntityFrameworkCore;
using Rybalka.Core.Entities;

namespace Rybalka.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<FishingNote> FishingNotes { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
