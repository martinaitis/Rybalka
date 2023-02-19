using AutoMapper;
using Microsoft.Extensions.Logging;
using Rybalka.Core.Dto.WeatherForecast;
using Rybalka.Core.Interfaces.Clients;
using System.Net.Http.Json;
using System.Web;

namespace Rybalka.Infrastructure.Clients.WeatherForecast
{
    public sealed class WeatherForecastClient : IWeatherForecastClient
    {
        private const string WEATHER_API_HISTORY_ENDPOINT = "https://api.weatherapi.com/v1/history.json";

        private readonly ILogger<WeatherForecastClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string? _weatherApiKey;

        public WeatherForecastClient(
            ILogger<WeatherForecastClient> logger,
            HttpClient httpClient,
            IMapper mapper)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(WEATHER_API_HISTORY_ENDPOINT);
            _mapper = mapper;

            _weatherApiKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY");
        }

        public async Task<HourWeatherForecastDto?> GetHourWeatherForecast(
            double latitude,
            double longitude,
            DateTime time,
            CancellationToken ct)
        {
            if (DateTime.Compare(time, DateTime.Now.AddDays(-7)) < 0)
            {
                _logger.LogWarning("Can not get older than 7 days weather forecast.");
                return null;
            }

            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["?key"] = _weatherApiKey;
                query["q"] = $"{latitude},{longitude}";
                query["dt"] = $"{time.Year}-{time.Month}-{time.Day}";
                query["hour"] = time.Hour.ToString();

                var forecastResponse = await _httpClient
                    .GetFromJsonAsync<WeatherForecastResponse>(query.ToString(), ct);

                var forecastDto = _mapper.Map<HourWeatherForecastDto>(
                    forecastResponse?.Forecast?.ForecastDay?[0].Hour?[0]);

                return forecastDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetHourWeatherForecast)} Error message: ", ex);
                return null;
            }
        }
    }
}
