using UniverseRepo.Application.Enums;

namespace UniverseRepo.Application.DTOs.Request;

public class RepositoryUpdateRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public RepositoryType Type { get; set; }
    public List<ContentRequestDto> Contents { get; set; }
}