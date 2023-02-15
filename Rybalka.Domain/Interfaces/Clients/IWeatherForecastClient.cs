using Rybalka.Domain.Dto.FishingNote;

namespace RybalkaWebAPI.Services.WeatherForecast
{
    public interface IWeatherForecastClient
    {
        public Task<HourWeatherForecastDto> GetHourWeatherForecast(
            double latitude,
            double longitude,
            DateTime time);
    }
}
