using Moq;
using Rybalka.Core.Entities;
using Rybalka.Core.Interfaces.Clients;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Core.Services;
using Rybalka.Test.Mocks.Database;
using Rybalka.Test.Utils;

namespace Rybalka.Test.TestServices
{
    public sealed class FishingNoteServiceTest
    {
        [Fact]
        public async void GetAllFishingNotesV2_AllFishingNotesReturn()
        {
            var fishingNoteRepositoryMock = new Mock<IFishingNoteRepository>();
            fishingNoteRepositoryMock.Setup(m => m.GetFishingNotesReadOnly(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MockFishingNoteTable.fishingNotes);
            var fishingNoteService = new FishingNoteService(
                Mappers.GetFishingNoteMapper(),
                fishingNoteRepositoryMock.Object,
                new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetAllFishingNotes(CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(MockFishingNoteTable.fishingNotes.Count, result.Count);
        }

        [Fact]
        public async void GetFishingNoteByIdV2_WhenIdExist_ThenFishingNoteReturn()
        {
            var fishingNoteRepositoryMock = new Mock<IFishingNoteRepository>();
            fishingNoteRepositoryMock
                .Setup(m => m.GetFishingNoteByIdReadOnly(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MockFishingNoteTable.fishingNotes.First());
            var fishingNoteService = new FishingNoteService(
               Mappers.GetFishingNoteMapper(),
               fishingNoteRepositoryMock.Object,
               new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetFishingNoteById(1, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async void GetFishingNoteByIdV2_WhenIdNotExist_ThenNullReturn()
        {
            var fishingNoteRepositoryMock = new Mock<IFishingNoteRepository>();
            fishingNoteRepositoryMock
                .Setup(m => m.GetFishingNoteByIdReadOnly(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);
            var fishingNoteService = new FishingNoteService(
               Mappers.GetFishingNoteMapper(),
               fishingNoteRepositoryMock.Object,
               new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetFishingNoteById(1, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async void GetFishingNotesByUserV2_WhenUserExist_ThenFishingNotesReturn()
        {
            var fishingNoteRepositoryMock = new Mock<IFishingNoteRepository>();
            fishingNoteRepositoryMock
                .Setup(m => m.GetFishingNotesByUserIdReadOnly(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MockFishingNoteTable.fishingNotes.GetRange(0, 1));
            var fishingNoteService = new FishingNoteService(
               Mappers.GetFishingNoteMapper(),
               fishingNoteRepositoryMock.Object,
               new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetFishingNotesByUserId(1, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async void GetFishingNotesByUserV2_WhenUserNotExist_ThenEmptyListReturn()
        {
            var fishingNoteRepositoryMock = new Mock<IFishingNoteRepository>();
            fishingNoteRepositoryMock
                .Setup(m => m.GetFishingNotesByUserIdReadOnly(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<FishingNote>());
            var fishingNoteService = new FishingNoteService(
               Mappers.GetFishingNoteMapper(),
               fishingNoteRepositoryMock.Object,
               new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetFishingNotesByUserId(99, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
