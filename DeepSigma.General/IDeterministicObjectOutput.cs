using DeepSigma.General.Serialization;
using System.Security.Cryptography;

namespace DeepSigma.General;

/// <summary>
/// Provides functionality to convert objects to a deterministic byte array representation.
/// Note: This implementation ensures that the JSON serialization is consistent by sorting properties alphabetically.
/// </summary>
public interface IDeterministicObjectOutput
{
    /// <summary>
    /// Converts an object to a deterministic byte array representation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public byte[] ToDeterministicBytes() => DeterministicSerializer.ToDeterministicBytes(this);

    /// <summary>
    /// Converts an object to a hash using the specified hash algorithm.
    /// </summary>
    /// <param name="hashAlgorithmName"></param>
    /// <returns></returns>
    public byte[] ToDeterministicHash(HashAlgorithmName hashAlgorithmName) => DeterministicSerializer.ToDeterministicHash(this, hashAlgorithmName);
}


/// <summary>
/// Extensions for object with Deterministic Object Output functionality.
/// </summary>
public static class IDeterministicObjectOutputExtensions
{
    extension(IDeterministicObjectOutput source)
    {
        /// <summary>
        /// Converts an object to a deterministic byte array representation.
        /// </summary>
        /// <returns></returns>
        public byte[] ToDeterministicBytes()
        {
            return source.ToDeterministicBytes();
        }

        /// <summary>
        /// Converts an object to a hash using the specified hash algorithm.
        /// </summary>
        /// <param name="hashAlgorithmName"></param>
        /// <returns></returns>
        public byte[] ToDeterministicHash(HashAlgorithmName hashAlgorithmName)
        {
            return source.ToDeterministicHash(hashAlgorithmName);
        }
    }
}