namespace weather.Infrastructure.Crypto;

using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using weather.Application.Interfaces;

public sealed class CryptoService : ICryptoService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public CryptoService(IOptions<CryptoOptions> options)
    {
        var opt = options.Value;
        _key = Convert.FromBase64String(opt.KeyBase64);
        _iv = Convert.FromBase64String(opt.IvBase64);
        if (_key.Length != 32) throw new ArgumentException("Key must be 32 bytes (AES-256)");
        if (_iv.Length != 16) throw new ArgumentException("IV must be 16 bytes");
    }

    public string Encrypt(string plaintext)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plaintext);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(cipherBytes);
    }

    public string Decrypt(string ciphertext)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        var cipherBytes = Convert.FromBase64String(ciphertext);
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }
}