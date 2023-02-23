using AutoMapper;
using Rybalka.Core.Dto.FishingNote;
using Rybalka.Core.Entities;

namespace Rybalka.Core.AutoMapper
{
    public sealed class FishingNoteProfile : Profile
    {
        private const string API_IMAGE_PATH = "https://sheikasop-001-site1.atempurl.com/images/";
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
                .ForPath(dest => dest.Coordinates!.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => string.Concat(API_IMAGE_PATH, src.ImageFileName)));
            CreateMap<FishingNoteRequest, FishingNote>()
                .ForPath(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates!.Latitude))
                .ForPath(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates!.Longitude));
        }
    }
}
