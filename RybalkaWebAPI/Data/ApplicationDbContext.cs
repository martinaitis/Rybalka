using Microsoft.EntityFrameworkCore;
using RybalkaWebAPI.Models.Entity;

namespace RybalkaWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<FishingNote> FishingNotes { get; set;}

        public DbSet<User> Users { get; set;}
    }
}
