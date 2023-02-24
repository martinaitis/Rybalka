using Moq;
using Rybalka.Core.Interfaces.Clients;

namespace Rybalka.Test.Mocks.Clients
{
    public sealed class MockIWeatherForecastClient
    {
        public static Mock<IWeatherForecastClient> GetMock()
        {
            var mock = new Mock<IWeatherForecastClient>();

            return mock;
        }

    }
}
