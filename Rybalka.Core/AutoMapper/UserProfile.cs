using AutoMapper;
using Rybalka.Core.Dto.User;
using Rybalka.Core.Entities;

namespace Rybalka.Core.AutoMapper
{
    public sealed class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
