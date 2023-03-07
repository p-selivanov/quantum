using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quantum.Lib.Common;

namespace Quantum.Lib.AspNet;

public class CustomJsonContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        
        if (property.PropertyType.IsAssignableTo(typeof(ISpecifiable)))
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
