using AutoMapper;
using Rybalka.Core.Dto.FishingNote;
using Rybalka.Core.Entities;

namespace Rybalka.Core.AutoMapper
{
    public sealed class FishingNoteProfile : Profile
    {
        public FishingNoteProfile()
        {
            CreateMap<FishingNoteDto, FishingNote>()
                .ForPath(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates!.Latitude))
                .ForPath(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates!.Longitude));
            CreateMap<FishingNote, FishingNoteDto>()
                .ForPath(dest => dest.Coordinates!.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForPath(dest => dest.Coordinates!.Longitude, opt => opt.MapFrom(src => src.Longitude));

            CreateMap<FishingNote, FishingNoteResponse>()
                .ForPath(dest => dest.Coordinates!.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForPath(dest => dest.Coordinates!.Longitude, opt => opt.MapFrom(src => src.Longitude));
            CreateMap<FishingNoteRequest, FishingNote>()
                .ForPath(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates!.Latitude))
                .ForPath(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates!.Longitude));
        }
    }
}
