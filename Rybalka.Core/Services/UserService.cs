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
        public async Task<List<UserDto>> GetAllUsers(CancellationToken ct)
        {
            var users = await _userRepository.GetUsersReadOnly(ct);

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task CreateUser(UserDto userDto, CancellationToken ct)
        {
            var user = _mapper.Map<User>(userDto);
            await _userRepository.CreateUser(user, ct);
        }

        public async Task<bool> DeleteUser(int id, CancellationToken ct)
        {
            var user = await _userRepository.GetUserById(id, ct);
            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteUser(user, ct);

            return true;
        }
    }
}
