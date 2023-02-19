using Rybalka.Core.Dto.User;

namespace Rybalka.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task CreateUser(UserDto userDto);
        Task<bool> DeleteUser(int id);
    }
}
