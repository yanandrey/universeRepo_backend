using UniverseRepo.Application.Enums;

namespace UniverseRepo.Application.DTOs.Request;

public class UserUpdateRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
}