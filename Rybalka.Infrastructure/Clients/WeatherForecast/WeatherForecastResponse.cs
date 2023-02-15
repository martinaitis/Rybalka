using System.Text.Json.Serialization;

namespace Rybalka.Infrastructure.Clients.WeatherForecast
{
    public sealed class WeatherForecastResponse
    {
        [JsonPropertyName("location")]
        public Location? Location { get; set; }

        [JsonPropertyName("forecast")]
        public Forecast? Forecast { get; set; }
    }

    public sealed class Location
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("lat")]
        public double? Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double? Longitude { get; set; }

        [JsonPropertyName("tz_id")]
        public string? TimeZone { get; set; }

        [JsonPropertyName("localtime_epoch")]
        public int? LocalTimeEpoch { get; set; }

        [JsonPropertyName("localtime")]
        public string? LocalTime { get; set; }
    }

    public sealed class Forecast
    {
        [JsonPropertyName("forecastday")]
        public List<ForecastDay>? ForecastDay { get; set; }
    }

    public sealed class ForecastDay
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("date_epoch")]
        public int? DateEpoch { get; set; }

        [JsonPropertyName("day")]
        public Day? Day { get; set; }

        [JsonPropertyName("astro")]
        public Astro? Astro { get; set; }

        [JsonPropertyName("hour")]
        public List<Hour>? Hour { get; set; }
    }

    public sealed class Day
    {
        [JsonPropertyName("maxtemp_c")]
        public decimal? MaxTemp { get; set; }

        [JsonPropertyName("mintemp_c")]
        public decimal? MinTemp { get; set; }

        [JsonPropertyName("avgtemp_c")]
        public decimal? AvgTemp { get; set; }

        [JsonPropertyName("maxwind_kph")]
        public decimal? MaxWindKph { get; set; }

        [JsonPropertyName("condition")]
        public Condition? Condition { get; set; }
    }

    public sealed class Condition
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("code")]
        public int? Code { get; set; }
    }

    public sealed class Astro
    {
        [JsonPropertyName("sunrise")]
        public string? Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public string? Sunset { get; set; }

        [JsonPropertyName("moon_phase")]
        public string? MoonPhase { get; set; }
    }

    public sealed class Hour
    {
        [JsonPropertyName("time_epoch")]
        public int? TimeEpoch { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("temp_c")]
        public decimal? Temp { get; set; }

        [JsonPropertyName("condition")]
        public Condition? Condition { get; set; }

        [JsonPropertyName("wind_kph")]
        public decimal? WindKph { get; set; }

        [JsonPropertyName("wind_dir")]
        public string? WindDir { get; set; }

        [JsonPropertyName("cloud")]
        public decimal? CloudPct { get; set; }
    }
}
