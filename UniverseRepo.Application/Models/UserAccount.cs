using UniverseRepo.Application.Enums;

namespace UniverseRepo.Application.Models;

public class UserAccount
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public AccountStatus Status { get; set; }
    public DateTime? LastEmailSent { get; set; }
    public DateTime CreatedAt { get; set; }
}