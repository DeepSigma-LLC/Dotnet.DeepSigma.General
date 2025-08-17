using DeepSigma.General.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General
{
    /// <summary>
    /// Methods for class extension for JSON serialization and deserialization.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IJSONIO<T>
    {
        /// <summary>
        /// Exports the current instance of the class to a JSON string.
        /// </summary>
        /// <returns></returns>
        public string ExportToJSON()
        {
            return SerializationUtilities.GetSerializedString(this);
        }

        /// <summary>
        /// Creates an instance of the class from a JSON string.
        /// </summary>
        /// <param name="JSONText"></param>
        /// <returns></returns>
        public static T? CreateFromJSON(string JSONText)
        {
            return SerializationUtilities.GetDeserializedObject<T>(JSONText);
        }
    }
}
