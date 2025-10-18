using Xunit;
using DeepSigma.General.Utilities;
using System.Security.Cryptography;

namespace DeepSigma.General.Tests.Tests;

public class Crypto_Tests
{
    [Fact]
    public void AESTest()
    {
        (byte[] key, byte[] IV) keys = Crypto.GenerateAESKeyAndIV(32, 16);
        string message = "This is a secret message.";
        byte[] encrypted = Crypto.AESEncrypt(message, keys.key, keys.IV);
        string decrypted = Crypto.AESDecrypt(encrypted, keys.key, keys.IV);
        string encrypted_text = System.Text.Encoding.UTF8.GetString(encrypted);
        
        Assert.Equal(message, decrypted);
        Assert.NotEqual(message, encrypted_text);
    }

    [Fact]
    public void RSATest()
    {
        (RSAParameters pub, RSAParameters priv) keys = Crypto.GenerateRSAKeys(2048);
        string message = "This is a secret message.";
        byte[] encrypted = Crypto.RSAEncrypt(message, keys.pub);
        byte[] decrypted = Crypto.RSADecrypt(encrypted, keys.priv);
        string decrypted_text = System.Text.Encoding.UTF8.GetString(decrypted);

        Assert.Equal(message, decrypted_text);
        string encrypted_string = System.Text.Encoding.UTF8.GetString(encrypted);
        Assert.NotEqual(message, encrypted_string);
    }

    [Fact]
    public void EllipticCurveSignitureShouldBeTrue()
    {
        (ECDsa pubic, ECDsa priv) wrong_key = Crypto.GenerateECDsaKeys();
        (ECDsa pubic, ECDsa priv) keys = Crypto.GenerateECDsaKeys();
        string message = "This is a secret message.";

        string signature = Crypto.EllipticCurveDigitalSignData(message, keys.priv);
        bool valid_sig = Crypto.EllipticCurveDigitalVerifyData(message, signature, keys.pubic);
        bool invalid_sig = Crypto.EllipticCurveDigitalVerifyData(message, signature, wrong_key.pubic);

        Assert.True(valid_sig);
        Assert.False(invalid_sig);
    }
}
