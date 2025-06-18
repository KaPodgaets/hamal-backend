using Hamal.Application.Common.Interfaces;

namespace Hamal.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        // In a real application, use a strong hashing algorithm like BCrypt or Argon2.
        // This is a simplification for the exercise.
        return $"hashed_{password}";
    }

    public bool VerifyPassword(string password, string hashedPassword) => $"hashed_{password}" == hashedPassword;
} 