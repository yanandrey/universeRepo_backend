using UniverseRepo.Application.Enums;

namespace UniverseRepo.Application.DTOs.Response;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public DateTime MemberSince { get; set; }
}