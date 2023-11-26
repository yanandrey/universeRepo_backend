using UniverseRepo.Application.Enums;

namespace UniverseRepo.Application.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    public UserAccount Account { get; set; }
}