using Hamal.Domain.Enums;

namespace Hamal.Domain.Entities;

public class User
{
    public Guid Id { get; init; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsDisabled { get; set; }
} 