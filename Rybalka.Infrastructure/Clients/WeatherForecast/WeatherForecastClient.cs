using AutoMapper;
using Rybalka.Core.Dto.WeatherForecast;
using Rybalka.Core.Interfaces.Clients;
using System.Net.Http.Json;
using System.Web;

namespace Rybalka.Infrastructure.Clients.WeatherForecast
{
    public sealed class WeatherForecastClient : IWeatherForecastClient
    {
        const string WEATHER_API_KEY = "10737a0a703044aa8e1223304232501"; //TODO: Move to config
        const string WEATHER_API_HISTORY_ENDPOINT = "https://api.weatherapi.com/v1/history.json";
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public WeatherForecastClient(
            HttpClient httpClient,
            IMapper mapper)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(WEATHER_API_HISTORY_ENDPOINT);
            _mapper = mapper;
        }

        public async Task<HourWeatherForecastDto?> GetHourWeatherForecast(
            double latitude,
            double longitude,
            DateTime time)
        {
            if (DateTime.Compare(time!, DateTime.Now.AddDays(-7)) < 0)
            {
                return null;
            }

            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["?key"] = WEATHER_API_KEY;
                query["q"] = $"{latitude},{longitude}";
                query["dt"] = $"{time.Year}-{time.Month}-{time.Day}";
                query["hour"] = time.Hour.ToString();

                var forecastResponse = await _httpClient.GetFromJsonAsync<WeatherForecastResponse>(query.ToString());
                var forecastDto = _mapper.Map<HourWeatherForecastDto>(forecastResponse?.Forecast?.ForecastDay?[0].Hour?[0]);

                return forecastDto;
            }
            catch (Exception ex) //TODO: Log exception
            {
                return null;
            }
        }
    }
}
