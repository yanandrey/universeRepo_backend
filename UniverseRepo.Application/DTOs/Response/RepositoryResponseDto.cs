using UniverseRepo.Application.Enums;

namespace UniverseRepo.Application.DTOs.Response;

public class RepositoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public RepositoryType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid OwnerId { get; set; }
    public List<ContentResponseDto> Contents { get; set; }
}