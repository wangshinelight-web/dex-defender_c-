using System;
using System.Security.Cryptography;

public static class AesCrypto
{
    private static readonly byte[] IV = new byte[16]; // 全零IV

    public static byte[] Encrypt(byte[] key, byte[] data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            using (var ms = new System.IO.MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }

    public static byte[] Decrypt(byte[] key, byte[] encrypted)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            using (var ms = new System.IO.MemoryStream(encrypted))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    byte[] decrypted = new byte[encrypted.Length];
                    int bytesRead = cs.Read(decrypted, 0, decrypted.Length);
                    Array.Resize(ref decrypted, bytesRead);
                    return decrypted;
                }
            }
        }
    }
}
