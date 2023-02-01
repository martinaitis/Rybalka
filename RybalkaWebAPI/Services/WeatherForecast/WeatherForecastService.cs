using System.Web;

namespace RybalkaWebAPI.Services.WeatherForecast
{
    public class WeatherForecastService : IWeatherForecastService
    {
        const string WEATHER_API_KEY_CONFIG = "WeatherApi:Key";
        const string WEATHER_API_HISTORY_ENDPOINT_CONFIG = "WeatherApi:HistoryEndpoint";
        private readonly HttpClient _httpClient;
        private readonly string _key;

        public WeatherForecastService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;

            var weatherHistoryEndpoint = config.GetValue<string>(WEATHER_API_HISTORY_ENDPOINT_CONFIG);
            if (weatherHistoryEndpoint == null)
            {
                throw new ArgumentNullException(nameof(WEATHER_API_HISTORY_ENDPOINT_CONFIG));
            }

            _httpClient.BaseAddress = new Uri(weatherHistoryEndpoint);

            var key = config.GetValue<string>(WEATHER_API_KEY_CONFIG);
            if (key == null)
            {
                throw new ArgumentNullException(nameof(WEATHER_API_KEY_CONFIG));
            }

            _key = key;
        }

        public async Task<Hour?> GetHourWeatherForecast(
            double latitude,
            double longitude,
            DateTime time)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["?key"] = _key;
            query["q"] = $"{latitude},{longitude}";
            query["dt"] = $"{time.Year}-{time.Month}-{time.Day}";
            query["hour"] = time.Hour.ToString();

            var weatherForecast = await _httpClient.GetFromJsonAsync<WeatherForecastResponse>(query.ToString());

            return weatherForecast?.Forecast?.ForecastDay?[0].Hour?[0];
        }        
    }
}
