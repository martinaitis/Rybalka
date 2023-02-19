using Rybalka.Core.Dto.WeatherForecast;

namespace Rybalka.Core.Interfaces.Clients
{
    public interface IWeatherForecastClient
    {
        public Task<HourWeatherForecastDto?> GetHourWeatherForecast(
            double latitude,
            double longitude,
            DateTime time,
            CancellationToken ct);
    }
}
