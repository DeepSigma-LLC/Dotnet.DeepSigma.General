using DeepSigma.General.Serialization;

namespace DeepSigma.General;

/// <summary>
/// Methods for class functionality extension for JSON serialization and deserialization.
/// </summary>
/// <typeparam name="T"></typeparam>
internal interface IJSONSerializer<T>
{
    /// <summary>
    /// Exports the current instance of the class to a JSON string.
    /// </summary>
    /// <returns></returns>
    internal string ToJSON()
    {
        return JsonSerializer.GetSerializedString(this);
    }

    /// <summary>
    /// Creates an instance of the class from a JSON string.
    /// </summary>
    /// <param name="JSONText"></param>
    /// <returns></returns>
    internal static T? Create(string JSONText)
    {
        return JsonSerializer.GetDeserializedObject<T>(JSONText);
    }
}

/// <summary>
///  Extensions for object with JSON Serialization. 
///  This is implemented to ensure that the complier shows the methods by default without the need to cast the object to the <see cref="IJSONSerializer{T}"/> interface. 
/// </summary>
internal static class IJSONSerializerExtensions
{
    extension<T>(IJSONSerializer<T> source) 
    {
        /// <summary>
        /// Exports the current instance of the class <typeparamref name="T"/> to a JSON string.
        /// </summary>
        /// <returns></returns>
        internal string ToJSON()
        {
            return source.ToJSON();
        }
    }

   
    extension<T>(IJSONSerializer<T>)
    {
        /// <summary>
        /// Creates an instance of <typeparamref name="T"/> from a JSON string.
        /// Note: T (<typeparamref name="T"/>) must have a parameterless constructor.
        /// </summary>
        /// <param name="JSON"></param>
        internal static T? Create(string JSON) => IJSONSerializer<T>.Create(JSON);
    }

    
}
