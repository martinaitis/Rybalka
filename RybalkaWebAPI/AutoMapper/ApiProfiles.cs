using AutoMapper;
using RybalkaWebAPI.Models;
using RybalkaWebAPI.Models.Dto;

namespace RybalkaWebAPI.AutoMapper
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
        }
    }
}
