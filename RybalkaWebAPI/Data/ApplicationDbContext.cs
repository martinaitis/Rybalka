using Microsoft.EntityFrameworkCore;
using RybalkaWebAPI.Models;

namespace RybalkaWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<FishingNote> FishingNotes { get; set;}

    }
}
