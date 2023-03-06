using AutoMapper;
using Rybalka.Core.AutoMapper;

namespace Rybalka.Test.Utils
{
    static class Mappers
    {
        public static Mapper GetFishingNoteMapper()
        {
            return new(new MapperConfiguration(cfg => cfg.AddProfile(new FishingNoteProfile())));
        }
    }
}
