using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Quantum.Lib.Common;

/// <summary>
/// A custom Newtonsoft Json contract resolver.
/// It handles writing of Specifiable fields.
/// </summary>
public class JsonContractResolver : DefaultContractResolver
{
    public JsonContractResolver()
    {
        NamingStrategy = new CamelCaseNamingStrategy
        {
            ProcessDictionaryKeys = true,
            OverrideSpecifiedNames = true,
        };
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        
        if (typeof(ISpecifiable).IsAssignableFrom(property.PropertyType))
        {
            property.ShouldSerialize = instance =>
            {
                var value = property.ValueProvider.GetValue(instance);
                var specifiable = (ISpecifiable)value;
                return specifiable?.IsSpecified == true;
            };
        }

        return property;
    }
}
