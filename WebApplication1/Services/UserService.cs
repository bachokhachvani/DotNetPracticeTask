using WebApplication1.Data;
using WebApplication1.Data.DTOs;
using WebApplication1.Repositories;
using WebApplication1.Utils;

namespace WebApplication1.Services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<User> GetUserById(int userId)
    {
        if (userId == 0)
        {
            throw new ArgumentException("User id cannot be zero");
        }

        var user = await _userRepository.FindUser(userId);
        if (user == null)
        {
            throw new ArgumentException("User does not exist");
        }

        return user;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _userRepository.GetUsers();
    }

    public async Task<ResponseDTO> AddUser(UserDTO user)
    {
        User newUser = UserMapper.UserDTOToUser(user);
        await _userRepository.AddUser(newUser);
        return new ResponseDTO("User added");
    }

    public async Task<ResponseDTO> UpdateUser(int id, UserDTO user)
    {
        await _userRepository.UpdateUser(id, user);
        return new ResponseDTO("User updated successfully");
    }

    public async Task<ResponseDTO> DeleteUser(int userId)
    {
        await _userRepository.DeleteUser(userId);
        return new ResponseDTO("User deleted successfully");
    }
}