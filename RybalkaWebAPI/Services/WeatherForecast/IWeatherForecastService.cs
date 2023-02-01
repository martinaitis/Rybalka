namespace RybalkaWebAPI.Services.WeatherForecast
{
    public interface IWeatherForecastService
    {
        public Task<Hour?> GetHourWeatherForecast(
            double latitude,
            double longitude,
            DateTime time);
    }
}
