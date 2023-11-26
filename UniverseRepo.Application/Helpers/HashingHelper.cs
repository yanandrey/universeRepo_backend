namespace UniverseRepo.Application.Helpers;

public static class HashingHelper
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public static bool ValidatePassword(string password, string correctHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, correctHash);
    }
}