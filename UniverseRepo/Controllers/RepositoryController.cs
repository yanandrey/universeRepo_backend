using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniverseRepo.Application.DTOs.Request;
using UniverseRepo.Application.DTOs.Response;
using UniverseRepo.Core.Helpers;
using UniverseRepo.Core.Repositories;

namespace UniverseRepo.Controllers;

[ApiController]
[Route("repository")]
[ApiExplorerSettings(IgnoreApi = false, GroupName = "Repository")]
public class RepositoryController : ControllerBase
{
    private readonly IRepositoryRepository _repository;
    private readonly ITokenHelper _tokenHelper;

    public RepositoryController(
        IRepositoryRepository repository,
        ITokenHelper tokenHelper)
    {
        _repository = repository;
        _tokenHelper = tokenHelper;
    }
    
    /// <summary>
    /// Return own repositories.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<RepositoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOwnRepositories()
    {
        var requesterId = Guid.Parse(_tokenHelper.GetRequesterId(Request.Headers["Authorization"]));
        var response = await _repository.GetMineAsync(requesterId);
        return Ok(response);
    }
    
    /// <summary>
    /// Return public repository by id.
    /// </summary>
    /// <param name="repositoryId">Repository id</param>
    /// <returns></returns>
    [HttpGet("{repositoryId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(RepositoryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRepositoryById(
        [FromRoute] Guid repositoryId)
    {
        var response = await _repository.GetByIdAsync(repositoryId);
        return Ok(response);
    }
    
    /// <summary>
    /// Return public repositories by name.
    /// </summary>
    /// <param name="repositoryName">Repository name</param>
    /// <returns></returns>
    [HttpGet("{repositoryName}")]
    [Authorize]
    [ProducesResponseType(typeof(List<RepositoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRepositoryByName(
        [FromRoute] string repositoryName)
    {
        var response = await _repository.GetByNameAsync(repositoryName);
        return Ok(response);
    }
    
    /// <summary>
    /// Register new repository.
    /// </summary>
    /// <param name="dto">Object to create a new repository</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(RepositoryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterRepository(
        [FromBody] RepositoryRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var requesterId = Guid.Parse(_tokenHelper.GetRequesterId(Request.Headers["Authorization"]));
        var response = await _repository.RegisterAsync(dto, requesterId);
        return Ok(response);
    }
    
    /// <summary>
    /// Update a repository.
    /// </summary>
    /// <param name="dto">Object to update a new repository</param>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(RepositoryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRepository(
        [FromBody] RepositoryUpdateRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();
        
        var response = await _repository.UpdateAsync(dto);
        return Ok(response);
    }
    
    /// <summary>
    /// Delete a repository.
    /// </summary>
    /// <param name="id">Repository id</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRepository(
        [FromRoute] Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
    
    /// <summary>
    /// Update a repository contents.
    /// </summary>
    /// <param name="dto">Object to update a repository contents</param>
    /// <returns></returns>
    [HttpPut("content")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateContentsRepository(
        [FromBody] ContentUpdateRequestDto dto)
    {
        var response = await _repository.UpdateContentAsync(dto);
        return Ok(response);
    }
}