using System.Security.Cryptography;
using Hamal.Application.Common.Interfaces;

namespace Hamal.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bit
    private const int HashSize = 32; // 256 bit
    private const int Iterations = 100_000;
    
    public string HashPassword(string password)
    {
        // Generate a secure salt
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Hash the password with the salt
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );

        // Store as: iterations.salt.hash (all base64)
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        // Parse hash parts
        var parts = hashedPassword.Split('.');
        if (parts.Length != 3) return false;

        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] hash = Convert.FromBase64String(parts[2]);

        // Recalculate hash from input password
        byte[] testHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );

        // Compare in constant time
        return CryptographicOperations.FixedTimeEquals(hash, testHash);
    }
} 