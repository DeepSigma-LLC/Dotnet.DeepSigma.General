using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSigma.General.Utilities;

/// <summary>
/// Provides utility methods for encoding and decoding data in various formats such as Hex, Base64, UTF-8, ASCII, Base58, and Base32.
/// </summary>
public class EncodingUtilities
{
    /// <summary>
    /// Converts a byte array to its hexadecimal string representation.
    /// </summary>
    /// <param name="byteArray"></param>
    /// <returns></returns>
    public static string ByteArrayToHexString(byte[] byteArray)
    {
        return Convert.ToHexStringLower(byteArray);
    }

    /// <summary>
    /// Converts a hexadecimal string to its corresponding byte array.
    /// </summary>
    /// <param name="hexString"></param>
    /// <returns></returns>
    public static byte[] HexStringToByteArray(string hexString)
    {
        return Convert.FromHexString(hexString);
    }

    /// <summary>
    /// Converts a byte array to its lowercase hexadecimal string representation.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ConvertToHexStringLower(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }

    /// <summary>
    /// Converts a Base64 encoded string to its corresponding byte array.
    /// </summary>
    /// <param name="base64String"></param>
    /// <returns></returns>
    public static byte[] Base64StringToByteArray(string base64String)
    {
        return Convert.FromBase64String(base64String);
    }

    /// <summary>
    /// Converts a byte array to its Base64 encoded string representation.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ConvertToBase64String(byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Converts a byte array to its UTF-8 string representation.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ConvertToUtf8String(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }


    /// <summary>
    /// Converts a UTF-8 string to its corresponding byte array.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] Utf8StringToByteArray(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    /// <summary>
    /// Converts a byte array to its ASCII string representation.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ConvertToAsciiString(byte[] bytes)
    {
        return Encoding.ASCII.GetString(bytes);
    }

    /// <summary>
    /// Converts an ASCII string to its corresponding byte array.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] AsciiStringToByteArray(string str)
    {
        return Encoding.ASCII.GetBytes(str);
    }

    /// <summary>
    /// Converts a byte array to its Base58 encoded string representation.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ConvertToBase58(byte[] bytes)
    {
        return Encode.Base58.Encode(bytes);
    }

    /// <summary>
    /// Converts a Base58 encoded string to its corresponding byte array.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] Base58StringToByteArray(string str)
    {
        return Encode.Base58.Decode(str);
    }

    /// <summary>
    /// Converts a byte array to its Base32 encoded string representation.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ConvertToBase32(byte[] bytes)
    {
        return Encode.Base32.Encode(bytes);
    }

    /// <summary>
    /// Converts a Base32 encoded string to its corresponding byte array.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] Base32StringToByteArray(string str)
    {
        return Encode.Base32.Decode(str);
    }
}
