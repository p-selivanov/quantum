using System;
using System.Linq;
using Newtonsoft.Json;

namespace Quantum.Lib.Common;

/// <summary>
/// A converter to read and write Specifiable field to Json.
/// </summary>
public class SpecifiableConverter : JsonConverter
{
    public override bool CanRead => true;

    public override bool CanWrite => true;

    public override bool CanConvert(Type objectType)
    {
        return typeof(ISpecifiable).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var realObjectType = objectType.GetGenericArguments().First();
        var value = serializer.Deserialize(reader, realObjectType);
        var instance = Activator.CreateInstance(objectType, value, true);
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
