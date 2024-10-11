using WebApplication1.Data;
using WebApplication1.Data.DTOs;

namespace WebApplication1.Utils;

public static class UserMapper
{
    public static User UserDTOToUser(UserDTO userDTO)
    {
        User user = new User();
        user.FirstName = userDTO.firstName;
        user.LastName = userDTO.lastName;
        return user;
    }

    public static UserDTO UserToUserDTO(User user)
    {
        UserDTO userDto = new UserDTO();
        userDto.firstName = user.FirstName;
        userDto.lastName = user.LastName;
        return userDto;
    }
}