using Newtonsoft.Json;

namespace DeepSigma.General.Utilities
{
    public static class SerializationUtilities
    {
        public static string GetSerializedString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T? GetDeserializedObject<T>(string JSONString)
        {
            return JsonConvert.DeserializeObject<T>(JSONString);
        }
    }
}
