using DeepSigma.General.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General
{
    public class JSONIO<T>
    {
        public string ExportTpJSON()
        {
            return SerializationUtilities.GetSerializedString(this);
        }

        public static T? CreateFromJSON(string JSONText)
        {
            return SerializationUtilities.GetDeserializedObject<T>(JSONText);
        }
    }
}
