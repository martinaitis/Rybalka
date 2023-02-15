namespace Rybalka.Core.Dto.WeatherForecast
{
    public sealed class HourWeatherForecastDto
    {
        public decimal? Temp { get; set; }
        public ConditionDto? Condition { get; set; }
        public decimal? WindKph { get; set; }
        public string? WindDir { get; set; }
        public decimal? CloudPct { get; set; }
    }

    public sealed class ConditionDto
    {
        public string? Text { get; set; }
    }
}
