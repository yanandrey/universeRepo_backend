using UniverseRepo.Application.Enums;

namespace UniverseRepo.Application.Models;

public class Repository
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public RepositoryType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public User Owner { get; set; }
}