namespace DAL.Extensions;

public static class Encryption
{
    private static readonly string Salt = BCrypt.Net.BCrypt.GenerateSalt(4);
    
    public static string Encrypt(string plainText) => BCrypt.Net.BCrypt.HashPassword(plainText, Salt);
    public static bool Verify(string plainText, string hash) => BCrypt.Net.BCrypt.Verify(plainText, hash);
}