using AutoMapper;
using Rybalka.Core.Dto.WeatherForecast;
using Rybalka.Infrastructure.Clients.WeatherForecast;

namespace Rybalka.Infrastructure.AutoMapper
{
    public sealed class WeatherForecastProfile : Profile
    {
        public WeatherForecastProfile()
        {
            CreateMap<Hour, HourWeatherForecastDto>();
            CreateMap<Condition, ConditionDto>();
        }
    }
}
