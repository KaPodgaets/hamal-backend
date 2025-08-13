using Hamal.Domain.Enums;

namespace Hamal.Domain.Entities;

public class User
{
    private User(
        Guid id,
        string username,
        string passwordHash,
        Role role,
        bool isDisabled = false)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        IsDisabled = isDisabled;
    }
    public Guid Id { get; init; }
    public string Username { get; private set; }
    public string PasswordHash { get;private set; }
    public Role Role { get;private set; }
    public bool IsDisabled { get;private set; }

    public static User Create(string username, string passwordHash, Role role)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(passwordHash));
        }
        
        return new User(Guid.NewGuid(), username, passwordHash, role);
    }
    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void Disable()
    {
        IsDisabled = true;   
    }

    public void Enable()
    {
        IsDisabled = false;  
    }
    
} 