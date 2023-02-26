using Microsoft.EntityFrameworkCore;
using Rybalka.Infrastructure.Data;
using Rybalka.Test.Mocks.Database;

namespace Rybalka.Test.Fixtures
{
    public sealed class ApplicationDbContextFixture : IDisposable
    {
        public ApplicationDbContext ApplicationDbContext { get; set; }
        public ApplicationDbContextFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ApplicationDbContext")
                .Options;

            var context = new ApplicationDbContext(options);
            context.FishingNotes.AddRange(MockFishingNoteTable.fishingNotes);
            context.SaveChanges();

            ApplicationDbContext = context;
        }

        public void Dispose()
        {
            ApplicationDbContext.Dispose();
        }
    }
}
