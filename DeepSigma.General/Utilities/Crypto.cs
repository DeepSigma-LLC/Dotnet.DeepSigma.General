
using System.Security.Cryptography;

namespace DeepSigma.General.Utilities;

/// <summary>
/// Provides utility methods for cryptographic operations such as RSA and AES encryption/decryption, key generation, and digital signatures.
/// </summary>
public static class Crypto
{
    /// <summary>
    /// Generates a new RSA key pair.
    /// </summary>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public static (RSAParameters publicKey, RSAParameters privateKey) GenerateRSAKeys(int keySize = 2048)
    {
        using (RSA rsa = RSA.Create(keySize))
        {
            return (rsa.ExportParameters(false), rsa.ExportParameters(true));
        }
    }

    /// <summary>
    /// Encrypts the given plain text using the provided RSA public key.
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="publicKey"></param>
    /// <returns></returns>
    public static string RSAEncrypt(string plainText, RSAParameters publicKey)
    {
        using RSA rsa = RSA.Create();
        rsa.ImportParameters(publicKey);
        byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// Decrypts the given encrypted text using the provided RSA private key.
    /// </summary>
    /// <param name="encryptedText"></param>
    /// <param name="privateKey"></param>
    /// <returns></returns>
    public static string RSADecrypt(string encryptedText, RSAParameters privateKey)
    {
        using RSA rsa = RSA.Create();
        rsa.ImportParameters(privateKey);

        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
        byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
        return System.Text.Encoding.UTF8.GetString(decryptedBytes);
    }

    /// <summary>
    /// Decrypts the given cipher text using the provided AES key and IV.
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static string AESDecrypt(string cipherText, byte[] key, byte[] iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using var ms = new System.IO.MemoryStream(cipherBytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new System.IO.StreamReader(cs);
        return sr.ReadToEnd();
    }

    /// <summary>
    /// Encrypts the given plain text using the provided AES key and IV.
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static string AESEncrypt(string plainText, byte[] key, byte[] iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new System.IO.MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new System.IO.StreamWriter(cs);
        sw.Write(plainText);
        sw.Flush();
        cs.FlushFinalBlock();
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// Generates a new AES key and IV.
    /// </summary>
    /// <param name="keySize"> Key size in bytes (16, 24, or 32 for AES-128, AES-192, or AES-256)</param>
    /// <param name="ivSize"> IV size in bytes (should be 16 for AES)</param>
    /// <returns></returns>
    public static (byte[] key, byte[] iv) GenerateAESKeyAndIV(int keySize = 32, int ivSize = 16)
    {
        using Aes aes = Aes.Create();
        aes.KeySize = keySize * 8; // bits
        aes.BlockSize = ivSize * 8; // bits
        aes.GenerateKey();
        aes.GenerateIV();
        return (aes.Key, aes.IV);
    }

    /// <summary>
    /// Signs the given data using the provided ECDsa private key.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="privateKey"></param>
    /// <returns></returns>
    public static string EllipticCurveDigitalSignData(string data, ECDsa privateKey)
    {
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
        byte[] signature = privateKey.SignData(dataBytes, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// Verifies the given signature for the data using the provided ECDsa public key.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signatureBase64"></param>
    /// <param name="publicKey"></param>
    /// <returns></returns>
    public static bool EllipticCurveDigitalVerifyData(string data, string signatureBase64, ECDsa publicKey)
    {
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
        byte[] signatureBytes = Convert.FromBase64String(signatureBase64);
        return publicKey.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static (ECDsa publicKey, ECDsa privateKey) GenerateECDsaKeys()
    {
        ECDsa privateKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        ECDsa publicKey = ECDsa.Create(privateKey.ExportParameters(false));
        return (publicKey, privateKey);
    }
}
