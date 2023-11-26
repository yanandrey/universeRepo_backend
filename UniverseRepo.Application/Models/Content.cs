namespace UniverseRepo.Application.Models;

public class Content
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
    public Repository Repository { get; set; }
}