using AutoMapper;
using Moq;
using Rybalka.Core.AutoMapper;
using Rybalka.Core.Entities;
using Rybalka.Core.Interfaces.Clients;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Core.Services;
using Rybalka.Test.Mocks.Database;

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
                GetFishingNoteMapper(),
                fishingNoteRepositoryMock.Object,
                new Mock<IWeatherForecastClient>().Object);
            
            var result = await fishingNoteService.GetAllFishingNotesV2(CancellationToken.None);

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
               GetFishingNoteMapper(),
               fishingNoteRepositoryMock.Object,
               new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetFishingNoteByIdV2(1, CancellationToken.None);

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
               GetFishingNoteMapper(),
               fishingNoteRepositoryMock.Object,
               new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetFishingNoteByIdV2(1, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async void GetFishingNotesByUserV2_WhenUserExist_ThenFishingNotesReturn()
        {
            var fishingNoteRepositoryMock = new Mock<IFishingNoteRepository>();
            fishingNoteRepositoryMock
                .Setup(m => m.GetFishingNotesByUserReadOnly("Karolis", It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MockFishingNoteTable.fishingNotes.GetRange(0, 1));
            var fishingNoteService = new FishingNoteService(
               GetFishingNoteMapper(),
               fishingNoteRepositoryMock.Object,
               new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetFishingNotesByUserV2("Karolis", CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async void GetFishingNotesByUserV2_WhenUserNotExist_ThenEmptyListReturn()
        {
            var fishingNoteRepositoryMock = new Mock<IFishingNoteRepository>();
            fishingNoteRepositoryMock
                .Setup(m => m.GetFishingNotesByUserReadOnly(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<FishingNote>());
            var fishingNoteService = new FishingNoteService(
               GetFishingNoteMapper(),
               fishingNoteRepositoryMock.Object,
               new Mock<IWeatherForecastClient>().Object);

            var result = await fishingNoteService.GetFishingNotesByUserV2("", CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private static Mapper GetFishingNoteMapper()
        {
            return new(new MapperConfiguration(cfg => cfg.AddProfile(new FishingNoteProfile())));
        }
    }
}
