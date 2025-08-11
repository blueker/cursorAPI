namespace weather.Infrastructure.Crypto;

public sealed class CryptoOptions
{
    public string KeyBase64 { get; set; } = string.Empty; // 32 bytes base64
    public string IvBase64 { get; set; } = string.Empty;  // 16 bytes base64
}