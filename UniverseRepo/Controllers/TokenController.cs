using Microsoft.AspNetCore.Mvc;
using UniverseRepo.Application.DTOs.Request;
using UniverseRepo.Application.DTOs.Response;
using UniverseRepo.Core.Repositories;
using UniverseRepo.Core.Services;

namespace UniverseRepo.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = false, GroupName = "Token")]
public class TokenController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public TokenController(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    
    /// <summary>
    /// Return the access token.
    /// </summary>
    /// <param name="dto">User credentials</param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateToken(
        [FromBody] LoginRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        
        var user = await _userRepository.AuthenticateAsync(
            dto);
        
        var response = await _tokenService.BuildJwtSecurityToken(
            user);
        
        return Ok(response);
    }
}