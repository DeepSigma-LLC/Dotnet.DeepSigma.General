using DeepSigma.General.Utilities;
using System.Security.Cryptography;
using Xunit;

namespace DeepSigma.General.Tests.Tests;

public class HashUtilities_Tests
{
    [Fact]
    public static void ComputeHashShouldComputeCorrectHash_256()
    {
        string result = HashUtilities.ComputeHash("test", HashAlgorithmName.SHA256);
        string expected = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08";
        Assert.Equal(expected, result);
    }

    [Fact]
    public static void ComputeHashShouldComputeCorrectHash_MD5()
    {
        string result = HashUtilities.ComputeHash("test", HashAlgorithmName.MD5);
        string expected = "098f6bcd4621d373cade4e832627b4f6";
        Assert.Equal(expected, result);
    }
}
