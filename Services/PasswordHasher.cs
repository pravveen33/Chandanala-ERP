using System.Security.Cryptography;

namespace Chandanala.ERP.Web.Services;

/// <summary>
/// PBKDF2-SHA256 password hashing. Deliberately implemented with only
/// framework-provided crypto (System.Security.Cryptography) rather than a
/// package like BCrypt.Net or Microsoft.AspNetCore.Identity, so it doesn't
/// add a NuGet dependency. If Identity is added to this project later,
/// User.PasswordHash values written by this class should be migrated
/// (users will need to reset their password) rather than assumed compatible.
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    /// <summary>
    /// Produces "iterations.saltBase64.hashBase64" — self-describing so the
    /// iteration count can be increased later without breaking old hashes.
    /// </summary>
    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split('.', 3);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
        {
            return false;
        }

        byte[] salt, expectedHash;
        try
        {
            salt = Convert.FromBase64String(parts[1]);
            expectedHash = Convert.FromBase64String(parts[2]);
        }
        catch (FormatException)
        {
            return false;
        }

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, expectedHash.Length);
        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
