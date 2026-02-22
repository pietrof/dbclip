using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DBClip.Services;

public static class EncryptionHelper
{
    private static readonly byte[] Key;
    private static readonly byte[] Iv;

    static EncryptionHelper()
    {
        var machineId = Environment.MachineName + Environment.UserName;
        using var sha256 = SHA256.Create();
        Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(machineId + "DBClipKey"));
        Iv = sha256.ComputeHash(Encoding.UTF8.GetBytes(machineId + "DBClipIV")).Take(16).ToArray();
    }

    public static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;
        
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;
        
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }
        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public static string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;
        
        try
        {
            var buffer = Convert.FromBase64String(cipherText);
            
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = Iv;
            
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(buffer);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
        catch
        {
            return cipherText;
        }
    }
}
