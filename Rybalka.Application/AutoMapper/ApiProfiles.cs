using AutoMapper;
using Rybalka.Application.Dto.FishingNote;
using Rybalka.Application.Dto.User;
using Rybalka.Domain.Entities;

namespace Rybalka.Infrastructure.AutoMapper
{
    public class ApiProfiles : Profile
    {
        public ApiProfiles()
        {
            CreateMap<FishingNoteDto, FishingNote>()
                .ForPath(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates!.Latitude))
                .ForPath(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates!.Longitude));
            CreateMap<FishingNote, FishingNoteDto>()
                .ForPath(dest => dest.Coordinates!.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForPath(dest => dest.Coordinates!.Longitude, opt => opt.MapFrom(src => src.Longitude));

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<Hour, HourWeatherForecastDto>();
            CreateMap<Condition, ConditionDto>();
        }
    }
}
