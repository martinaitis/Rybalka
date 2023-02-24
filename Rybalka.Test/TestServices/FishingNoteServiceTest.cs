using AutoMapper;
using Rybalka.Core.AutoMapper;
using Rybalka.Core.Services;
using Rybalka.Test.Mocks.Clients;
using Rybalka.Test.Mocks.Repositories;

namespace Rybalka.Test.TestServices
{
    public sealed class FishingNoteServiceTest
    {
        private readonly Mapper _mapper = new(
            new MapperConfiguration(
                cfg => cfg.AddProfile(new FishingNoteProfile())));

        [Fact]
        public async void WhenGettingAllFishingNotes_ThenAllFishingNotesReturn()
        {
            var fishingNoteRepositoryMock = MockIFishingNoteRepository.GetMock();
            var weatherForecastClientMock = MockIWeatherForecastClient.GetMock();
            var fishingNoteService = new FishingNoteService(
                _mapper,
                fishingNoteRepositoryMock.Object,
                weatherForecastClientMock.Object);

            var result = await fishingNoteService.GetAllFishingNotes(CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
