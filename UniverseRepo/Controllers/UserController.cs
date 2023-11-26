using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniverseRepo.Application.DTOs.Request;
using UniverseRepo.Application.DTOs.Response;
using UniverseRepo.Core.Helpers;
using UniverseRepo.Core.Repositories;

namespace UniverseRepo.Controllers;

[ApiController]
[Route("user")]
[ApiExplorerSettings(IgnoreApi = false, GroupName = "User")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _user;
    private readonly ITokenHelper _tokenHelper;

    public UserController(
        ITokenHelper tokenHelper,
        IUserRepository userRepository)
    {
        _tokenHelper = tokenHelper;
        _user = userRepository;
    }
    
    /// <summary>
    /// Return own user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOwnUser()
    {
        var requesterId = Guid.Parse(_tokenHelper.GetRequesterId(Request.Headers["Authorization"]));
        var response = await _user.GetMeAsync(requesterId);
        return Ok(response);
    }
    
    /// <summary>
    /// Return user by id.
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns></returns>
    [HttpGet("{userId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid userId)
    {
        var response = await _user.GetByIdAsync(userId);
        return Ok(response);
    }
    
    /// <summary>
    /// Register new user.
    /// </summary>
    /// <param name="dto">Object to create a new user</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser(
        [FromBody] UserRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        
        var response = await _user.RegisterAsync(dto);
        return Ok(response);
    }
    
    /// <summary>
    /// Update user.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UserUpdateRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        
        var response = await _user.UpdateAsync(dto);
        return Ok(response);
    }
}