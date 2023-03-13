using Rybalka.Infrastructure.Repositories;
using Rybalka.Test.Fixtures;
using Rybalka.Test.Mocks.Database;

namespace Rybalka.Test.TestRepositories
{
    public sealed class FishingNoteRepositoryTest : IClassFixture<ApplicationDbContextFixture>
    {
        private readonly ApplicationDbContextFixture _dbFixture;

        public FishingNoteRepositoryTest(ApplicationDbContextFixture dbFixture)
        {
            _dbFixture = dbFixture;
        }

        [Fact]
        public async void GetFishingNotesReadOnly_AllFishingNotesReturn()
        {
            var fishingNoteRepository = new FishingNoteRepository(_dbFixture.ApplicationDbContext);

            var result = await fishingNoteRepository.GetFishingNotesReadOnly(CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(MockFishingNoteTable.fishingNotes.Count, result.Count);
        }

        [Fact]
        public async void GetFishingNoteByIdReadOnly_WhenIdExist_ThenFishingNoteReturn()
        {
            var fishingNoteRepository = new FishingNoteRepository(_dbFixture.ApplicationDbContext);

            var result = await fishingNoteRepository.GetFishingNoteByIdReadOnly(2, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async void GetFishingNoteByIdReadOnly_WhenIdNotExist_ThenNullReturn()
        {
            var fishingNoteRepository = new FishingNoteRepository(_dbFixture.ApplicationDbContext);

            var result = await fishingNoteRepository.GetFishingNoteByIdReadOnly(999, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async void GetFishingNoteById_WhenIdExist_ThenFishingNoteReturn()
        {
            var fishingNoteRepository = new FishingNoteRepository(_dbFixture.ApplicationDbContext);

            var result = await fishingNoteRepository.GetFishingNoteById(2, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async void GetFishingNoteById_WhenIdNotExist_ThenNullReturn()
        {
            var fishingNoteRepository = new FishingNoteRepository(_dbFixture.ApplicationDbContext);

            var result = await fishingNoteRepository.GetFishingNoteById(999, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async void GetFishingNoteByUserReadOnly_WhenUserExist_ThenFishingNotesReturn()
        {
            var fishingNoteRepository = new FishingNoteRepository(_dbFixture.ApplicationDbContext);

            var result = await fishingNoteRepository
                .GetFishingNotesByUserIdReadOnly(1, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);
        }

        [Fact]
        public async void GetFishingNoteByUserReadOnly_WhenUserNotExist_ThenEmptyListReturn()
        {
            var fishingNoteRepository = new FishingNoteRepository(_dbFixture.ApplicationDbContext);

            var result = await fishingNoteRepository
                .GetFishingNotesByUserIdReadOnly(99, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
