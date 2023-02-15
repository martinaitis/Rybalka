using AutoMapper;
using Rybalka.Core.Dto.User;
using Rybalka.Core.Entities;
using Rybalka.Core.Interfaces.Repositories;
using Rybalka.Core.Interfaces.Services;

namespace Rybalka.Core.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository) 
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetUsersReadOnly();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userRepository.CreateUser(user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteUser(user);

            return true;
        }

        public async Task<bool> Login(UserDto userDto)
        {
            var user = await _userRepository.GetUserByIdReadOnly(userDto.Id);
            if (userDto.Password != userDto.Password)
            {
                return false;
            }

            return true;
        }
    }
}
