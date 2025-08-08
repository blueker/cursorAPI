namespace weather.Application.Interfaces;

public interface ICryptoService
{
    string Encrypt(string plaintext);
    string Decrypt(string ciphertext);
}