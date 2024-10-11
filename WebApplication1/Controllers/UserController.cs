using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.DTOs;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]/sync")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;


    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ResponseDTO>> CreateUser([FromBody] UserDTO user)
    {
        var responseDto = await _userService.AddUser(user);
        return Ok(responseDto);
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await _userService.GetUsers();

        return Ok(users);
    }

    [HttpGet("{id}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<UserDTO>> GetUser([FromRoute] int id)
    {
        User user = await _userService.GetUserById(id);
        UserDTO userDTO = UserMapper.UserToUserDTO(user);
        return Ok(userDTO);
    }

    [HttpPut("{id}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ResponseDTO>> UpdateUser([FromRoute] int id, [FromBody] UserDTO user)
    {
        return await _userService.UpdateUser(id, user);
    }

    [HttpDelete("{id}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<ResponseDTO>> DeleteUser([FromRoute] int id)
    {
        return await _userService.DeleteUser(id);
    }
}