using System.Security.Cryptography;
using System.Text;

namespace AlaskaShop.Domain.Services.Crypto;

public class PasswordEncrypter
{
    private readonly string? _key;

    public PasswordEncrypter(string? key)
        => _key = key;

    public string Encrypt(string password)
    {
        var newPassword = $"{password}{_key ?? "@Encrypt"}";
        var bytes = Encoding.UTF8.GetBytes(newPassword);
        var hashBytes = SHA512.HashData(bytes);

        var sb = new StringBuilder();
        foreach(byte b in hashBytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
}
