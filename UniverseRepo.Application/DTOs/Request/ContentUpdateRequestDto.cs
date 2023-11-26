namespace UniverseRepo.Application.DTOs.Request;

public class ContentUpdateRequestDto
{
    public Guid Id { get; set; }
    public List<ContentItemsUpdateRequestDto> Contents { get; set; }
}

public class ContentItemsUpdateRequestDto
{
    public string Title { get; set; }
    public string Value { get; set; }
}