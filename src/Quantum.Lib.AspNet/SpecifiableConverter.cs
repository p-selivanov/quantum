using System;
using System.Linq;
using Newtonsoft.Json;
using Quantum.Lib.Common;

namespace Quantum.Lib.AspNet
{
    public class SpecifiableConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableTo(typeof(ISpecifiable));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var realObjectType = objectType.GetGenericArguments().First();
            var value = serializer.Deserialize(reader, realObjectType);
            var instance = Activator.CreateInstance(objectType, value);
            return instance;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var specifiable = (ISpecifiable)value;
            if (specifiable?.IsSpecified == true)
            {
                serializer.Serialize(writer, specifiable.Value);
            }
        }
    }
}
