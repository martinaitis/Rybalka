using AutoMapper;
using RybalkaWebAPI.Models;
using RybalkaWebAPI.Models.Dto;

namespace RybalkaWebAPI.AutoMapper
{
    public class ApiProfiles : Profile
    {
        public ApiProfiles()
        {
            CreateMap<FishingNoteDto, FishingNote>();
            CreateMap<FishingNote, FishingNoteDto>();
        }
    }
}
